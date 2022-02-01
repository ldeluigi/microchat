using System.Threading.Tasks;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Domain.Aggregates.UserService;

public interface IRegistrationMethod<T>
{
    Task<Result<User>> CreateUser(T userData);
}
