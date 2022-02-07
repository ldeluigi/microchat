using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mapping;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Web.Controllers.V_1_0.SignalRChatDtos;

public record PrivateChatDto(
    Guid Id,
    Guid? Creator,
    Guid? Partecipant,
    Timestamp Creation);

public class PrivateChatDtoMapping : DirectMapping<PrivateChatOutput, PrivateChatDto>
{
    public PrivateChatDtoMapping() : base(o => new(
        o.Id,
        o.CreatorId.AsNullable(),
        o.PartecipantId.AsNullable(),
        o.CreationTimestamp))
    {
    }
}
