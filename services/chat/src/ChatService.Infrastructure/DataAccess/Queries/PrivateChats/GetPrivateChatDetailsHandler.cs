using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatService.Application;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.Tools;
using System;
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

    private DetailedPrivateChatOutput ConvertModelToOutput(PrivateChatModel privateChat, Guid asSeenBy)
    {
        var chatUser = (asSeenBy.Equals(privateChat.CreatorId) || asSeenBy.Equals(privateChat.PartecipantId)) ? true : false;
        var numberOfUnreadMessages = chatUser ? Some(privateChat.Messages.Select(m => !m.Viewed && m.ChatId == privateChat.Id).Count()) : None;

        return new(
            Id: privateChat.Id,
            CreatorId: privateChat.CreatorId.AsOption(),
            PartecipantId: privateChat.PartecipantId.AsOption(),
            CreationTimestamp: privateChat.CreationTime,
            NumberOfMessages: privateChat.Messages.Select(m => m.ChatId == privateChat.Id).Count(),
            NumberOfUnreadMessages: numberOfUnreadMessages);
    }

    protected override async Task<Response<DetailedPrivateChatOutput>> Handle(GetPrivateChatDetails request) =>
        await _chatContext.PrivateChats
            .Where(c => c.Id == request.Id)
            .Select(c => ConvertModelToOutput(c, _userInfoProvider.RequireUserId()))
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
}
