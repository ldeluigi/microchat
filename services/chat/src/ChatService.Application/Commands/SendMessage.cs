using ChatService.Application.Queries;
using ChatService.Domain.MessageService;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Time;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands
{
    /// <summary>
    /// Request of create chat.
    /// </summary>
    public class SendMessage
    {
        /// <summary>
        /// The command data for the <see cref="SendMessage"/> command.
        /// </summary>
        /// <param name="MessageId">The message identificator.</param>
        /// <param name="ChatId">The chat identificator where this message is been sent.</param>
        /// <param name="Text">The text of this message.</param>
        /// <param name="Sender">The sender of this message.</param>
        public record Command(Guid MessageId, Guid ChatId, string Text, Guid Sender) : CommandBase<MessageOutput>;

        /// <summary>
        /// The handler for the <see cref="SendMessage"/> command.
        /// </summary>
        public class Handler : UnitOfWorkHandler<Command, MessageOutput>
        {
            private readonly MessageLifecycleService _messageLifecycleService;
            private readonly ITimestampProvider _timestampProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="messageLifecycleService">The message service class used for register the message.</param>
            /// <param name="timestampProvider">The timestamp provider.</param>
            /// <param name="unitOfWork">The unit of work.</param>
            public Handler(
                MessageLifecycleService messageLifecycleService,
                ITimestampProvider timestampProvider,
                IUnitOfWork unitOfWork) : base(unitOfWork)
            {
                _messageLifecycleService = messageLifecycleService;
                _timestampProvider = timestampProvider;
            }

            /// <inheritdoc/>
            protected async override Task<Response<MessageOutput>> HandleRequest(Command request)
            {
                return await _messageLifecycleService
                    .Register(
                    id: request.MessageId,
                    chatId: request.ChatId,
                    text: request.Text,
                    sender: request.Sender,
                    sendTime: _timestampProvider.Now)
                    .ThenMap(MessageOutput.From)
                    .ThenToResponse();
            }
        }
    }
}
