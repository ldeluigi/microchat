using ChatService.Application.Queries.PrivateChat;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using System;

namespace ChatService.Application.Queries.Chat;

public record GetPrivateChatsOfUser(Guid UserId, Pagination Pagination) :
    PaginatedQueryBase<PrivateChatOutput>(Pagination);
