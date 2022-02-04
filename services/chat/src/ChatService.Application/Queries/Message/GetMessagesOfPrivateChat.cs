using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using System;

namespace ChatService.Application.Queries.Message;

public record GetMessagesOfPrivateChat(Guid ChatId, Pagination Pagination) :
    PaginatedQueryBase<PrivateChatMessageOutput>(Pagination);
