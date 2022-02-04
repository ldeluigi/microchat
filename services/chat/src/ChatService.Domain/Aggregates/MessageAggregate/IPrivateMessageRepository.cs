using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;

namespace ChatService.Domain.Aggregates.MessageAggregate;
public interface IPrivateMessageRepository :
    IGetByIdRepository<PrivateMessage, Guid>,
    ISaveRepository<PrivateMessage>,
    IRemoveRepository<PrivateMessage>
{
}
