using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Domain;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using System;
using System.Threading.Tasks;

namespace ChatService.Application.Commands.PrivateChats;

public static class CreatePrivateChat
{
    public record Command(
       Guid Creator,
       Guid Partecipant) : CommandBase<PrivateChatOutput>;

    public class Handler : RequestHandlerBase<Command, PrivateChatOutput>
    {
        private readonly PrivateChatLifecycleService _privateChatLifecycleService;

        public Handler(
            PrivateChatLifecycleService privateChatLifecycleService)
        {
            _privateChatLifecycleService = privateChatLifecycleService;
        }

        protected override async Task<Response<PrivateChatOutput>> Handle(Command request)
        {
            return await _privateChatLifecycleService
                .CreatePrivateChat(request.Creator, request.Partecipant)
                .ThenToResponse()
                .ThenMap(PrivateChatOutput.From);
        }
    }
}
