using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Domain;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

namespace ChatService.Application.Commands.PrivateChats;

public class DeletePrivateChat
{
    public record Command(Guid ChatId) : CommandBase<PrivateChatOutput>;

    public class Handler : RequestHandlerBase<Command, PrivateChatOutput>
    {
        private readonly PrivateChatLifecycleService _privateChatLifecycleService;
        private readonly IUserInfoProvider _userInfoProvider;
        private readonly IPrivateChatRepository _privateChatRepository;

        public Handler(
            PrivateChatLifecycleService privateChatLifecycleService,
            IUserInfoProvider userInfoProvider,
            IPrivateChatRepository privateChatRepository)
        {
            _privateChatLifecycleService = privateChatLifecycleService;
            _userInfoProvider = userInfoProvider;
            _privateChatRepository = privateChatRepository;
        }

        protected override async Task<Response<PrivateChatOutput>> Handle(Command request)
        {
            var userId = _userInfoProvider.RequireUserId();
            return await _privateChatRepository
                .GetById(request.ChatId)
                .ThenRequire(chat =>
                {
                    if (!chat.PartecipantId.Contains(userId) && !chat.CreatorId.Contains(userId))
                    {
                        return Failure<PrivateChatOutput>(new NotFoundError());
                    }
                    return Success(chat);
                })
                .ThenFlatMapAsync(async (chat) =>
                    await _privateChatLifecycleService
                        .DeletePrivateChat(chat.Id)
                        .ThenToResponse()
                        .ThenMap(PrivateChatOutput.From));
        }
    }
}
