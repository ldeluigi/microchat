using ChatService.Application.Queries;
using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands;

public class EditMessage
{
    public record Command(Guid MessageId, string Text) : CommandBase<MessageOutput>;

    public class Handler : RequestHandlerBase<Command, MessageOutput>
    {
        private readonly IMessageRepository _messageRepository;

        public Handler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        // TODO migliorabile
        protected override async Task<Response<MessageOutput>> Handle(Command request)
        {
            return await _messageRepository.GetById(request.MessageId)
                /*.ThenIfSuccess(message => string.IsNullOrWhiteSpace(message.Text)
                    ? throw new ArgumentNullException(nameof(message.Text))
                    : message.EditText(request.Text))*/
                .ThenIfSuccess(message =>
                    {
                        if (string.IsNullOrWhiteSpace(request.Text))
                        {
                            throw new ArgumentNullException(nameof(request.Text));
                        }
                        else
                        {
                            message.EditText(request.Text);
                        }
                    })
                .ThenIfSuccess(message => _messageRepository.Save(message))
                .ThenMap(MessageOutput.From)
                .ThenToResponse();
        }
    }
}
