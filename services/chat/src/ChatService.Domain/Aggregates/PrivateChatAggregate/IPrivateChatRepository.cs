using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;
using System.Threading.Tasks;

namespace ChatService.Domain.Aggregates.PrivateChatAggregate;

public interface IPrivateChatRepository :
    IGetByIdRepository<PrivateChat, Guid>,
    ISaveRepository<PrivateChat>,
    IRemoveRepository<PrivateChat>
{
    Task<bool> ChatAlreadyExistBetween(Guid creatorId, Guid participantId);
}
