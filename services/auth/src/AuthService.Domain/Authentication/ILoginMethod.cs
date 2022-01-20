using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;

namespace AuthService.Domain.Authentication;

public interface ILoginMethod<T>
{
    Task<Result<Guid>> VerifyCredentials(T credentials);
}
