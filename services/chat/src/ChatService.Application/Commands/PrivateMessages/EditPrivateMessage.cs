using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Mediator.Handlers;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Time;
using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

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

    public class Handler : ICommandHandler<Command, PrivateChatMessageOutput>
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUserInfoProvider _userInfoProvider;
        private readonly ITimestampProvider _timestampProvider;

        public Handler(
            IPrivateMessageRepository privateMessageRepository,
            IPrivateChatRepository privateChatRepository,
            IUserInfoProvider userInfoProvider,
            ITimestampProvider timestampProvider)
        {
            _privateMessageRepository = privateMessageRepository;
            _privateChatRepository = privateChatRepository;
            _userInfoProvider = userInfoProvider;
            _timestampProvider = timestampProvider;
        }

        public async Task<Response<PrivateChatMessageOutput>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _userInfoProvider.RequireUserId();
            Response<PrivateChat> chatResult = default;
            return await _privateMessageRepository.GetById(request.MessageId)
                .ThenRequireAsync(async m => chatResult = await _privateChatRepository
                                                                    .GetById(m.ChatId)
                                                                    .ThenRequire(chat =>
                                                                    {
                                                                        if (!chat.PartecipantId.Contains(userId) && !chat.CreatorId.Contains(userId))
                                                                        {
                                                                            return Failure<PrivateChatMessageOutput>(new NotFoundError());
                                                                        }
                                                                        return Success(chat);
                                                                    }))
                .ThenIfSuccess(message => message.EditText(MessageText.From(request.Text), _timestampProvider.Now))
                .ThenIfSuccess(message => _privateMessageRepository.Save(message))
                .ThenMap(m => PrivateChatMessageOutput.From(m, chatResult.ReadValue()));
        }
    }
}
