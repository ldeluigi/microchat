using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace Microchat.ChatService.Application.Queries
{
    public record MessageSnapshot(
        Guid Id,
        Guid ChatId,
        string Text,
        Guid Sender,
        Timestamp RequestTimestamp,
        Option<Timestamp> LastEditTime);
}
