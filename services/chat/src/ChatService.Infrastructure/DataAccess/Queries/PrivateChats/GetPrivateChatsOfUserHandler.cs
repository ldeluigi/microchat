using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatService.Application;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.Tools.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
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

    private PrivateChatOfUserOutput ConvertModelToOutput(PrivateChatModel privateChat, Guid asSeenBy)
    {
        var chatUser = (asSeenBy.Equals(privateChat.CreatorId) || asSeenBy.Equals(privateChat.PartecipantId)) ? true : false;
        var numberOfUnreadMessages = chatUser ? Some(privateChat.Messages.Select(m => !m.Viewed && m.ChatId == privateChat.Id).Count()) : None;

        return new(
            Id: privateChat.Id,
            CreatorId: privateChat.CreatorId.AsOption(),
            PartecipantId: privateChat.PartecipantId.AsOption(),
            CreationTimestamp: privateChat.CreationTime,
            NumberOfUnreadMessages: numberOfUnreadMessages);
    }

    protected override async Task<Response<Page<PrivateChatOfUserOutput>>> Handle(GetPrivateChatsOfUser request) =>
        await _chatContext.PrivateChats
            .Where(c => c.CreatorId.Equals(request.UserId) || c.PartecipantId.Equals(request.UserId))
            .OrderByDescending(c => c.Messages.MaxBy(m => m.SendTime))
            .Select(c => ConvertModelToOutput(c, _userInfoProvider.RequireUserId()))
            .GetPage(request.Pagination);
}
