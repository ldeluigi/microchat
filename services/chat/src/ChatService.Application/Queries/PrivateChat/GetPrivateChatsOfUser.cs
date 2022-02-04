using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using System;

namespace ChatService.Application.Queries.PrivateChat;

public record GetPrivateChatsOfUser(Guid UserId, Pagination Pagination) :
    PaginatedQueryBase<PrivateChatOutput>(Pagination);
