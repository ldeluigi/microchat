using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;

namespace UserService.Domain.Aggregates.UserAggregate;

public interface IUserRepository :
    IGetByIdRepository<User, Guid>,
    ISaveRepository<User>,
    IRemoveRepository<User>
{
    public async Task<Result<User>> RemoveById(Guid id)
    {
        return await GetById(id)
            .ThenIfSuccess(Remove);
    }
}
