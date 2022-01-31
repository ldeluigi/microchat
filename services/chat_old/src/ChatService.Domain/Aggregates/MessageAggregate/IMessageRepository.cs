using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;

namespace ChatService.Domain.Aggregates.MessageAggregate;
    public interface IMessageRepository :
    IGetByIdRepository<Message, Guid>,
    ISaveRepository<Message>,
    IRemoveRepository<Message>
    {
    }
