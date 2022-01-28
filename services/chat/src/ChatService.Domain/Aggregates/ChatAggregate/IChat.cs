using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Domain.Aggregates.ChatAggregate
{
    internal interface IChat
    {
        Guid Id { get; }

        Guid Owner { get; }

        Timestamp CreationTime { get; }
    }
}
