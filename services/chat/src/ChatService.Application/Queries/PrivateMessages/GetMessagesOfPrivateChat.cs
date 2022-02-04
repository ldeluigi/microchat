using ChatService.Application.Queries.PrivateMessages.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using System;

namespace ChatService.Application.Queries.PrivateMessages;

public record GetMessagesOfPrivateChat(Guid ChatId, Pagination Pagination) :
    PaginatedQueryBase<PrivateChatMessageOutput>(Pagination);
