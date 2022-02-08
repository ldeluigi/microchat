using System.Linq;
using System.Threading.Tasks;
using ChatService.Application;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Infrastructure.DataAccess.Queries.PrivateChats.Mappers;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.Tools.Options;
using Microsoft.EntityFrameworkCore;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatsOfUserHandler : PaginatedQueryHandlerBase<GetPrivateChatsOfUser, PrivateChatOfUserOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IUserInfoProvider _userInfoProvider;

    public GetPrivateChatsOfUserHandler(ChatContext chatContext, IUserInfoProvider userInfoProvider)
    {
        _chatContext = chatContext;
        _userInfoProvider = userInfoProvider;
    }

    protected override async Task<Response<Page<PrivateChatOfUserOutput>>> Handle(GetPrivateChatsOfUser request)
    {
        var userId = _userInfoProvider.RequireUserId();
        if (userId != request.UserId)
        {
            return Failure<Page<PrivateChatOfUserOutput>>(new NotFoundError());
        }
        return await _chatContext.PrivateChats
            .AsNoTracking()
            .Where(c => c.CreatorId == userId || c.PartecipantId == userId)
            .OrderBy(c => c.Id)
            .GroupJoin(
                _chatContext.PrivateMessages,
                on => on.Id,
                on => on.ChatId,
                (chat, messages) =>
                    PrivateChatModelMapper
                        .ConvertModelToOutput(
                            chat,
                            messages.Select(m => !m.Viewed && m.SenderId != userId).Count(),
                            userId))
            .GetPageAsync(request.Pagination);
    }
}
