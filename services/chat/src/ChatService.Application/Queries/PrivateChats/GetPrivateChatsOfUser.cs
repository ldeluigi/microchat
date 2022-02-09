using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using System;

namespace ChatService.Application.Queries.PrivateChats;

public record GetPrivateChatsOfUser(Guid UserId, Pagination Pagination) :
    QueryWithPaginationBase<PrivateChatOfUserOutput>(Pagination);
