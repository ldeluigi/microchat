using System;
using System.Threading.Tasks;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;

namespace AuthService.Domain.Authentication;

public interface ILoginMethod<T>
{
    Task<Result<Guid>> VerifyCredentials(T credentials);
}
