using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Domain.Chat;

internal interface IChat
{
    Guid Id { get; }

    Option<Guid> CreatorId { get; }

    Timestamp CreationTime { get; }
}
