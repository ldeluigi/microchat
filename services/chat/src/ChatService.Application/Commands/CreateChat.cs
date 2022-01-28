using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatService.Application.Queries;
using ChatService.Domain.Aggregates.ChatAggregate;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Time;

namespace ChatService.Application.Commands
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
        public record Command(IEnumerable<Guid> Partecipants) : CommandBase<PrivateChatOutput>;

        /// <summary>
        /// The handler for the <see cref="CreateChat"/> command.
        /// </summary>
        public class Handler : UnitOfWorkHandler<Command, PrivateChatOutput>
        {
            private readonly IPrivateChatRepository _chatRepository;
            private readonly ITimestampProvider _timestampProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="chatRepository">The chat repository.</param>
            /// <param name="timestampProvider">The timestamp provider.</param>
            /// <param name="unitOfWork">The unit of work.</param>
            public Handler(
                IPrivateChatRepository chatRepository,
                ITimestampProvider timestampProvider,
                IUnitOfWork unitOfWork) : base(unitOfWork)
            {
                _chatRepository = chatRepository;
                _timestampProvider = timestampProvider;
            }

            /// <inheritdoc/>
            protected override Task<Response<PrivateChatOutput>> HandleRequest(Command request)
            {
                throw new NotImplementedException();
            }
        }
    }
}
