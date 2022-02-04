using ChatService.Application.Queries.PrivateChat;
using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace ChatService.Application.Queries.Chat;

public record GetPrivateChatDetails(Guid Id) : QueryBase<DetailedPrivateChatOutput>;
