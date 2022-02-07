using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Time;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands.PrivateMessages;

public class EditPrivateMessage
{
    public record Command(Guid MessageId, string Text) : CommandBase<PrivateChatMessageOutput>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Text)
                .MaximumLength(MessageText.MaximumLength)
                .NotEmpty();
        }
    }

    public class Handler : RequestHandlerBase<Command, PrivateChatMessageOutput>
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;
        private readonly IUserInfoProvider _userInfoProvider;
        private readonly ITimestampProvider _timestampProvider;

        public Handler(
            IPrivateMessageRepository privateMessageRepository,
            IUserInfoProvider userInfoProvider,
            ITimestampProvider timestampProvider)
        {
            _privateMessageRepository = privateMessageRepository;
            _userInfoProvider = userInfoProvider;
            _timestampProvider = timestampProvider;
        }

        protected override async Task<Response<PrivateChatMessageOutput>> Handle(Command request) =>
            await _privateMessageRepository.GetById(request.MessageId)
                .ThenIfSuccess(message => message.EditText(MessageText.From(request.Text), _timestampProvider.Now))
                .ThenIfSuccess(message => _privateMessageRepository.Save(message))
                .ThenMap(m => PrivateChatMessageOutput.From(m, _userInfoProvider.RequireUserId()))
                .ThenToResponse();
    }
}
