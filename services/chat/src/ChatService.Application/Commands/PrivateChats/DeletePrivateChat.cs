using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Domain;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands.PrivateChats;

public class DeletePrivateChat
{
    public record Command(Guid ChatId) : CommandBase<PrivateChatOutput>;

    public class Handler : RequestHandlerBase<Command, PrivateChatOutput>
    {
        private readonly PrivateChatLifecycleService _privateChatLifecycleService;

        public Handler(PrivateChatLifecycleService privateChatLifecycleService)
        {
            _privateChatLifecycleService = privateChatLifecycleService;
        }

        protected override Task<Response<PrivateChatOutput>> Handle(Command request) =>
                _privateChatLifecycleService
                    .DeletePrivateChat(request.ChatId)
                    .ThenToResponse()
                    .ThenMap(PrivateChatOutput.From);
    }
}
