using System;
using System.Threading.Tasks;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public interface IAccountRepository :
    ISaveRepository<Account>,
    IRemoveRepository<Account>
{
    Task<Result<Account>> GetById(Guid id);

    Task<Result<Account>> GetByEmail(Email email);

    Task<Result<Account>> GetByUsername(Username username);

    Task<Result<Account>> GetByRefreshToken(Token token);

    Task<bool> EmailExists(Email email);

    Task<bool> UsernameExists(Username email);

    Task<(bool EmailExists, bool UsernameExists)> EmailOrUsernameExists(Email email, Username username);
}
