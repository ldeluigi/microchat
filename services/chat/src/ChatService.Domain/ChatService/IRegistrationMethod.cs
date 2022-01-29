using ChatService.Domain.Aggregates.ChatAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System.Threading.Tasks;

namespace ChatService.Domain.ChatService
{
    public interface IRegistrationMethod<T>
    {
        Task<Result<PrivateChat>> CreatePrivateChat(T chatData);
    }
}
