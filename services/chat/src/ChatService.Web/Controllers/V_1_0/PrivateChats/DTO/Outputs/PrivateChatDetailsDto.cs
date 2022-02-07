using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mapping;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Web.Controllers.V_1_0.PrivateChats.DTO.Outputs;

public record PrivateChatDetailsDto(
    Guid Id,
    Guid? Creator,
    Guid? Partecipant,
    Timestamp Creation,
    int NumberOfMessages);

public class PrivateChatDetailsMapping : DirectMapping<DetailedPrivateChatOutput, PrivateChatDetailsDto>
{
    public PrivateChatDetailsMapping() : base(o => new(
        o.Id,
        o.CreatorId.AsNullable(),
        o.PartecipantId.AsNullable(),
        o.CreationTimestamp,
        o.NumberOfMessages))
    {
    }
}
