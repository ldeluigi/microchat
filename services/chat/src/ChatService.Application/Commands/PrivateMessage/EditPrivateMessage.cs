using ChatService.Application.Queries.PrivateMessage;
using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands.Message;

public class EditPrivateMessage
{
    public record Command(Guid MessageId, string Text) : CommandBase<PrivateChatMessageOutput>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Text).MaximumLength(MessageText.MaximumLength);
            RuleFor(c => c.Text).NotEmpty();
        }
    }

    public class Handler : RequestHandlerBase<Command, PrivateChatMessageOutput>
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;

        public Handler(
            IPrivateMessageRepository privateMessageRepository) => _privateMessageRepository = privateMessageRepository;

        protected override async Task<Response<PrivateChatMessageOutput>> Handle(Command request) =>
            await _privateMessageRepository.GetById(request.MessageId)
                .ThenIfSuccess(message => message.EditText(new MessageText(request.Text)))
                .ThenIfSuccess(message => _privateMessageRepository.Save(message))
                .ThenMap(PrivateChatMessageOutput.From)
                .ThenToResponse();
    }
}
