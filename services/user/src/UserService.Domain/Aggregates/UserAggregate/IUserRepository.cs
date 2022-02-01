using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Domain.Aggregates.UserAggregate;

/// <summary>
/// Repository for Message.
/// </summary>
public interface IUserRepository :
    IGetByIdRepository<User, Guid>,
    ISaveRepository<User>,
    IRemoveRepository<User>
{
    Task<Result<User>> GetByEmail(Email email);
}
