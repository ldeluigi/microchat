using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;

namespace ChatService.Domain.Chat;

internal interface IChat
{
    Guid Id { get; }

    Guid Creator { get; }

    Timestamp CreationTime { get; }
}
