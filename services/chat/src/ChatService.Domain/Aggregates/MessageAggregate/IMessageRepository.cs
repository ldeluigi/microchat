using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;

namespace Microchat.ChatService.Domain.Aggregates.MessageAggregate
{
    /// <summary>
    /// Repository for Message.
    /// </summary>
    public interface IMessageRepository :
        IGetByIdRepository<Message, Guid>,
        ISaveRepository<Message>,
        IRemoveRepository<Message>
    {
    }
}
