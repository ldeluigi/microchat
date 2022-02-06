﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatService.Application;
using ChatService.Application.Queries.PrivateMessages;
using ChatService.Application.Queries.PrivateMessages.Outputs;
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

    public GetMessagesOfPrivateChatHandler(ChatContext chatContext, IUserInfoProvider userInfoProvider)
    {
        _chatContext = chatContext;
        _userInfoProvider = userInfoProvider;
    }

    private PrivateChatMessageOutput ConvertModelToOutput(PrivateMessageModel privateMessage, Guid asSeenBy) => new(
        Id: privateMessage.Id,
        ChatId: privateMessage.ChatId,
        SendTime: privateMessage.SendTime,
        LastEditTime: privateMessage.LastEditTime,
        SenderId: privateMessage.SenderId.AsOption(),
        Viewed: privateMessage.SenderId.AsOption().Contains(asSeenBy) ? true : privateMessage.Viewed,
        Text: privateMessage.Text);

    protected override async Task<Response<Page<PrivateChatMessageOutput>>> Handle(GetMessagesOfPrivateChat request) =>
        await _chatContext.PrivateMessages
            .Where(m => m.ChatId == request.ChatId)
            .OrderByDescending(m => m.SendTime)
            .Select(m => ConvertModelToOutput(m, _userInfoProvider.RequireUserId()))
            .GetPage(request.Pagination);
}