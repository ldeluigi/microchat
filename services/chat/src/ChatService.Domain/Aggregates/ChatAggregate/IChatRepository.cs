﻿using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;

namespace Microchat.ChatService.Domain.Aggregates.ChatAggregate
{
    /// <summary>
    /// Repository for Chat.
    /// </summary>
    public interface IChatRepository :
        ISaveRepository<Chat>
    {
    }
}