using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mapping;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Web.Controllers.V_1_0.PrivateChats.DTO.Outputs;

public record PrivateChatOfUserDto(
    Guid Id,
    Guid? Creator,
    Guid? Partecipant,
    Timestamp Creation,
    int? NumberOfUnreadMessages);

public class PrivateChatOfUserMapping : DirectMapping<PrivateChatOfUserOutput, PrivateChatOfUserDto>
{
    public PrivateChatOfUserMapping() : base(o => new(
        o.Id,
        o.CreatorId.AsNullable(),
        o.PartecipantId.AsNullable(),
        o.CreationTimestamp,
        o.NumberOfUnreadMessages.AsNullable()))
    {
    }
}
