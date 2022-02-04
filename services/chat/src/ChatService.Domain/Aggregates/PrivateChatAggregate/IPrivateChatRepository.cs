using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;

namespace ChatService.Domain.Aggregates.PrivateChatAggregate;

/// <summary>
/// Repository for Chat.
/// </summary>
public interface IPrivateChatRepository :
    IGetByIdRepository<PrivateChat, Guid>,
    ISaveRepository<PrivateChat>,
    IRemoveRepository<PrivateChat>
{
}
