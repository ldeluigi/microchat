using ChatService.Application.Queries.PrivateMessages.Outputs;
using EasyDesk.CleanArchitecture.Application.Mapping;
using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Web.Controllers.V_1_0.SignalRChatDtos;

public record PrivateMessageDto(
    Guid Id,
    string Text,
    Guid Chat,
    Guid? Sender,
    Timestamp SendTime,
    Timestamp LastEditTime,
    bool Viewed);

public class PrivateMessageDtoMapping : DirectMapping<PrivateChatMessageOutput, PrivateMessageDto>
{
    public PrivateMessageDtoMapping() : base(msg => new(
        msg.Id,
        msg.Text,
        msg.ChatId,
        msg.SenderId.AsNullable(),
        msg.SendTime,
        msg.LastEditTime.OrElseNull(),
        msg.Viewed))
    {
    }
}
