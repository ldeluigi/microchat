﻿using System;
using System.Threading.Tasks;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Time;
using Microchat.ChatService.Application.Queries;
using Microchat.ChatService.Domain.Aggregates.MessageAggregate;

namespace Microchat.ChatService.Application.Commands
{
    /// <summary>
    /// Request of create chat.
    /// </summary>
    public static class SendMessage
    {
        /// <summary>
        /// The command data for the <see cref="CreateChat"/> command.
        /// </summary>
        /// <param name="ChatId">The chat identificator where this message is been sent.</param>
        /// <param name="Text">The text of this message.</param>
        /// <param name="Sender">The sender of this message.</param>
        public record Command(Guid ChatId, string Text, Guid Sender) : CommandBase<MessageSnapshot>;

        /// <summary>
        /// The handler for the <see cref="CreateChat"/> command.
        /// </summary>
        public class Handler : UnitOfWorkHandler<Command, MessageSnapshot>
        {
            private readonly IMessageRepository _messageRepository;
            private readonly ITimestampProvider _timestampProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="messageRepository">The message repository.</param>
            /// <param name="timestampProvider">The timestamp provider.</param>
            /// <param name="unitOfWork">The unit of work.</param>
            public Handler(
                IMessageRepository messageRepository,
                ITimestampProvider timestampProvider,
                IUnitOfWork unitOfWork) : base(unitOfWork)
            {
                _messageRepository = messageRepository;
                _timestampProvider = timestampProvider;
            }

            /// <inheritdoc/>
            protected override Task<Response<MessageSnapshot>> HandleRequest(Command request)
            {
                var message = Message.Create(request.ChatId, request.Text, request.Sender, _timestampProvider.Now);
                _messageRepository.Save(message);
                return Task.FromResult<Response<MessageSnapshot>>(new MessageSnapshot(
                    message.Id,
                    message.ChatId,
                    message.Text,
                    message.Sender,
                    message.SendTime,
                    message.LastEditTime));
            }
        }
    }
}