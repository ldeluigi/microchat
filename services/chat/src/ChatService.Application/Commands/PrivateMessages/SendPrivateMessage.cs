using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Time;
using FluentValidation;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Application.Commands.PrivateMessages;

public class SendPrivateMessage
{
    public record Command(
        Guid ChatId,
        Guid SenderId,
        bool Viewed,
        string Text) : CommandBase<PrivateChatMessageOutput>;

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
        private readonly ITimestampProvider _timestampProvider;

        public Handler(
            IPrivateMessageRepository privateMessageRepository,
            ITimestampProvider timestampProvider)
        {
            _privateMessageRepository = privateMessageRepository;
            _timestampProvider = timestampProvider;
        }

        protected override Task<Response<PrivateChatMessageOutput>> Handle(Command request)
        {
            var message = PrivateMessage.Create(
                id: Guid.NewGuid(),
                chatId: request.ChatId,
                text: MessageText.From(request.Text),
                senderId: request.SenderId,
                sendTime: _timestampProvider.Now);
            _privateMessageRepository.Save(message);
            return Task.FromResult(Success(message))
                .ThenMap(PrivateChatMessageOutput.From)
                .ThenToResponse();
        }
    }
}
