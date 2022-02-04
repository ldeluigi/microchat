using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace ChatService.Application.Queries.PrivateChat;

public record GetPrivateChatDetails(Guid Id) : QueryBase<DetailedPrivateChatOutput>;
