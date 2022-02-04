using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace ChatService.Application.Queries.PrivateChats;

public record GetPrivateChatDetails(Guid Id) : QueryBase<DetailedPrivateChatOutput>;
