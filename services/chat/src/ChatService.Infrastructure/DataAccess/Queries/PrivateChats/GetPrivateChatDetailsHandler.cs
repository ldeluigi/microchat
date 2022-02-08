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
        return await _chatContext.PrivateMessages
            .AsNoTracking()
            .Join(
                _chatContext.PrivateChats
                    .Where(c => c.Id == request.Id && (c.PartecipantId == userId || c.CreatorId == userId)),
                on => on.ChatId,
                on => on.Id,
                (message, chat) => new { Chat = chat, Message = message })
            .GroupBy(
                x => x.Chat.Id,
                (chatId, group) =>
                    new { Chat = group.First().Chat, MessageCount = group.Count() })
            .FirstOptionAsync()
            .ThenMap(x => new DetailedPrivateChatOutput(
                x.Chat.Id,
                x.Chat.CreatorId.AsOption(),
                x.Chat.PartecipantId.AsOption(),
                x.Chat.CreationTime,
                x.MessageCount))
            .ThenOrElseError(Errors.NotFound);
    }
}
