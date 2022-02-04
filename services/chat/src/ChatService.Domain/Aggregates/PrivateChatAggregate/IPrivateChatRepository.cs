using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;

namespace ChatService.Domain.Aggregates.PrivateChatAggregate;

public interface IPrivateChatRepository :
    IGetByIdRepository<PrivateChat, Guid>,
    ISaveRepository<PrivateChat>,
    IRemoveRepository<PrivateChat>
{
    void DeleteOrphanChats();
}
