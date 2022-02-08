using ChatService.Application;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatDetailsHandler : RequestHandlerBase<GetPrivateChatDetails, DetailedPrivateChatOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IUserInfoProvider _userInfoProvider;

    public GetPrivateChatDetailsHandler(ChatContext chatContext, IUserInfoProvider userInfoProvider)
    {
        _chatContext = chatContext;
        _userInfoProvider = userInfoProvider;
    }

    protected override async Task<Response<DetailedPrivateChatOutput>> Handle(GetPrivateChatDetails request)
    {
        var userId = _userInfoProvider.RequireUserId();
        return await _chatContext.PrivateChats
            .AsNoTracking()
            .Where(c => c.Id == request.Id && (c.PartecipantId == userId || c.CreatorId == userId))
            .Select(c => new { MessageCount = _chatContext.PrivateMessages.Where(m => m.ChatId == c.Id).Count(), Chat = c })
            .Select(x => new DetailedPrivateChatOutput(
                x.Chat.Id,
                x.Chat.CreatorId.AsOption(),
                x.Chat.PartecipantId.AsOption(),
                x.Chat.CreationTime,
                x.MessageCount))
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
    }
}
