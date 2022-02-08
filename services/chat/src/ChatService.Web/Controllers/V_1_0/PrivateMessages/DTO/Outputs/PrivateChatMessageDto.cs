using ChatService.Application.Queries.PrivateMessages.Outputs;
using EasyDesk.CleanArchitecture.Application.Mapping;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Web.Controllers.V_1_0.PrivateMessages.DTO.Outputs;

public record PrivateChatMessageDto(
    Guid Id,
    Guid? Sender,
    Guid Chat,
    Timestamp Timestamp,
    Timestamp LastEdit,
    string Text,
    bool Viewed);

public class PrivateChatMessageMapping : DirectMapping<PrivateChatMessageOutput, PrivateChatMessageDto>
{
    public PrivateChatMessageMapping() : base(o => new(
        o.Id,
        o.SenderId.AsNullable(),
        o.ChatId,
        o.SendTime,
        o.LastEditTime.OrElseNull(),
        o.Text,
        o.Viewed))
    {
    }
}
