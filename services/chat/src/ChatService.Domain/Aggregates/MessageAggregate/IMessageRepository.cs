using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;

namespace Microchat.UserService.Domain.Aggregates.UserAggregate
{
    /// <summary>
    /// Repository for Message.
    /// </summary>
    public interface IUserRepository :
        IGetByIdRepository<User, Guid>,
        ISaveRepository<User>,
        IRemoveRepository<User>
    {
    }
}
