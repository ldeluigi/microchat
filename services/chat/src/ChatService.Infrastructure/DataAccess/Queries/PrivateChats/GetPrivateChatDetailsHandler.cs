using ChatService.Application;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
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
            .Where(c => c.Id == request.Id && (c.PartecipantId == userId || c.CreatorId == userId))
            .GroupJoin(
                _chatContext.PrivateMessages,
                on => on.Id,
                on => on.ChatId,
                (chat, messages) => new { Chat = chat, Messages = messages.Count() })
            .FirstOptionAsync()
            .ThenMap(res => new DetailedPrivateChatOutput(
                res.Chat.Id,
                res.Chat.CreatorId.AsOption(),
                res.Chat.PartecipantId.AsOption(),
                res.Chat.CreationTime,
                res.Messages))
            .ThenOrElseError(Errors.NotFound);
    }
}
