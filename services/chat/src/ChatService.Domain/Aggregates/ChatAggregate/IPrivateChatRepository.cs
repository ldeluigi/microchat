using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;

namespace ChatService.Domain.Aggregates.ChatAggregate;

/// <summary>
/// Repository for Chat.
/// </summary>
public interface IPrivateChatRepository :
    IGetByIdRepository<PrivateChat, Guid>,
    ISaveRepository<PrivateChat>,
    IRemoveRepository<PrivateChat>
{
}
