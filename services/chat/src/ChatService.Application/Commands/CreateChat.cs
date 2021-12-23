using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Time;
using Microchat.ChatService.Application.Queries;
using Microchat.ChatService.Domain.ChatAggregate;

namespace Microchat.ChatService.Application.Commands
{
    /// <summary>
    /// Request of create chat.
    /// </summary>
    public static class CreateChat
    {
        /// <summary>
        /// The command data for the <see cref="CreateChat"/> command.
        /// </summary>
        /// <param name="Partecipants">The identifiers list of chat's partecipants.</param>
        public record Command(IEnumerable<Guid> Partecipants) : CommandBase<ChatSnapshot>;

        /// <summary>
        /// The handler for the <see cref="CreateChat"/> command.
        /// </summary>
        public class Handler : UnitOfWorkHandler<Command, ChatSnapshot>
        {
            private readonly IChatRepository _chatRepository;
            private readonly ITimestampProvider _timestampProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="chatRepository">The chat repository.</param>
            /// <param name="timestampProvider">The timestamp provider.</param>
            /// <param name="unitOfWork">The unit of work.</param>
            public Handler(
                IChatRepository chatRepository,
                ITimestampProvider timestampProvider,
                IUnitOfWork unitOfWork) : base(unitOfWork)
            {
                _chatRepository = chatRepository;
                _timestampProvider = timestampProvider;
            }

            /// <inheritdoc/>
            protected override Task<Response<ChatSnapshot>> HandleRequest(Command request)
            {
                var chat = Chat.Create(request.Partecipants, _timestampProvider.Now);
                _chatRepository.Save(chat);
                return Task.FromResult<Response<ChatSnapshot>>(new ChatSnapshot(
                    chat.Id,
                    chat.Partecipants,
                    chat.CreationTime));
            }
        }
    }
}
