using System;
using System.Collections.Generic;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;

namespace Microchat.ChatService.Application.Queries
{
    public record ChatSnapshot(
        Guid Id,
        IEnumerable<Guid> Partecipants,
        Timestamp CreationTime);
}
