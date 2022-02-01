using System;
using System.Threading.Tasks;
using ChatService.Application.Queries;
using ChatService.Domain.ChatService;
using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Time;

namespace ChatService.Application.Commands
{
    /// <summary>
    /// Request of create chat.
    /// </summary>
    public class RegisterPrivateChat
    {
        /// <summary>
        /// The command data for the <see cref="RegisterPrivateChat"/> command.
        /// </summary>
        /// <param name="ChatId">The identifier of chat.</param>
        /// <param name="Owner">The identifiers chat's owner.</param>
        /// <param name="Partecipant">The identifiers chat's partecipant.</param>
        public record Command(Guid ChatId, Guid Owner, Guid Partecipant) : CommandBase<PrivateChatOutput>;

        /// <summary>
        /// The handler for the <see cref="RegisterPrivateChat"/> command.
        /// </summary>
        public class Handler : UnitOfWorkHandler<Command, PrivateChatOutput>
        {
            private readonly PrivateChatLifecycleService _chatLifecycleService;
            private readonly ITimestampProvider _timestampProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="chatLifecycleService">The chat service.</param>
            /// <param name="timestampProvider">The timestamp provider.</param>
            /// <param name="unitOfWork">The unit of work.</param>
            public Handler(
                PrivateChatLifecycleService chatLifecycleService,
                ITimestampProvider timestampProvider,
                IUnitOfWork unitOfWork) : base(unitOfWork)
            {
                _chatLifecycleService = chatLifecycleService;
                _timestampProvider = timestampProvider;
            }

            /// <inheritdoc/>
            protected override async Task<Response<PrivateChatOutput>> HandleRequest(Command request)
            {
                return await _chatLifecycleService
                        .Register(
                        id: request.ChatId,
                        owner: request.Owner,
                        partecipant: request.Partecipant,
                        creationTime: _timestampProvider.Now)
                        .ThenMap(PrivateChatOutput.From)
                        .ThenToResponse();
             }
        }
    }
}
