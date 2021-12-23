using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace Microchat.ChatService.Application.Queries
{
    public record MessageSnapshot(
        Guid Id,
        string Text,
        Timestamp RequestTimestamp,
        Option<DateTime> LastEditTime,
        Guid Sender);
}
