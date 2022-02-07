using AutoMapper;
using ChatService.Application;
using ChatService.Application.Queries.PrivateMessages;
using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.Tools.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateMessages;

public class GetMessagesOfPrivateChatHandler : PaginatedQueryHandlerBase<GetMessagesOfPrivateChat, PrivateChatMessageOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IMapper _mapper;

    public GetMessagesOfPrivateChatHandler(ChatContext chatContext, IUserInfoProvider userInfoProvider, IMapper mapper)
    {
        _chatContext = chatContext;
        _userInfoProvider = userInfoProvider;
        _mapper = mapper;
    }

    private PrivateChatMessageOutput ConvertModelToOutput(PrivateMessageModel privateMessage, PrivateChatModel chatModel, Guid asSeenBy) => new(
        Id: privateMessage.Id,
        ChatId: privateMessage.ChatId,
        SendTime: privateMessage.SendTime,
        LastEditTime: privateMessage.LastEditTime.AsOption(),
        SenderId: privateMessage.SenderId.AsOption(),
        Viewed: privateMessage.SenderId.AsOption().Contains(asSeenBy) ? true : privateMessage.Viewed,
        Text: privateMessage.Text,
        Chat: new(
            Id: chatModel.Id,
            CreatorId: chatModel.CreatorId.AsOption(),
            PartecipantId: chatModel.PartecipantId.AsOption(),
            CreationTimestamp: chatModel.CreationTime));

    protected override async Task<Response<Page<PrivateChatMessageOutput>>> Handle(GetMessagesOfPrivateChat request)
    {
        var userId = _userInfoProvider.RequireUserId();
        return await _chatContext.PrivateMessages
            .Where(m => m.ChatId == request.ChatId)
            .OrderByDescending(m => m.SendTime)
            .Join(_chatContext.PrivateChats, on => on.ChatId, on => on.Id, (message, chat) => ConvertModelToOutput(message, chat, userId))
            .GetPageAsync(request.Pagination);
    }
}
