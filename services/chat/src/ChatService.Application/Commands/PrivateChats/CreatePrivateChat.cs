using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Domain;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Mediator.Handlers;
using EasyDesk.CleanArchitecture.Application.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Commands.PrivateChats;

public static class CreatePrivateChat
{
    public record Command(
       Guid Partecipant) : CommandBase<PrivateChatOutput>;

    public class Handler : ICommandHandler<Command, PrivateChatOutput>
    {
        private readonly IUserInfoProvider _userInfoProvider;
        private readonly PrivateChatLifecycleService _privateChatLifecycleService;

        public Handler(
            IUserInfoProvider userInfoProvider,
            PrivateChatLifecycleService privateChatLifecycleService)
        {
            _userInfoProvider = userInfoProvider;
            _privateChatLifecycleService = privateChatLifecycleService;
        }

        public async Task<Response<PrivateChatOutput>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await _privateChatLifecycleService
                .CreatePrivateChat(_userInfoProvider.RequireUserId(), request.Partecipant)
                .ThenToResponse()
                .ThenMap(PrivateChatOutput.From);
        }
    }
}
