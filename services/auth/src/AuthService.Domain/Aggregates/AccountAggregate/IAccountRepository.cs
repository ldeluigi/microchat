using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using System;
using System.Threading.Tasks;

namespace AuthService.Domain.Aggregates.AccountAggregate;

public interface IAccountRepository :
    ISaveRepository<Account>,
    IRemoveRepository<Account>
{
    Task<Result<Account>> GetById(Guid id);

    Task<Result<Account>> GetByEmail(Email email);

    Task<Result<Account>> GetByUsername(Username username);

    Task<Result<Account>> GetByPasswordRecoveryToken(Token token);

    Task<Result<Account>> GetByConfirmationToken(Token token);

    Task<Result<Account>> GetByRefreshToken(Token token);

    Task<bool> EmailExists(Email email);

    Task<(bool EmailExists, bool UsernameExists)> EmailOrUsernameExists(Email email, Username username);
}
