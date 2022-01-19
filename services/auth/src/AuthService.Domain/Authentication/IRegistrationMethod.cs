using System.Threading.Tasks;
using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;

namespace AuthService.Domain.Authentication;

public interface IRegistrationMethod<T>
{
    Task<Result<Account>> CreateAccount(T accountData);
}
