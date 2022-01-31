using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System.Threading.Tasks;

namespace ChatService.Domain.MessageService;

public interface IRegistrationMethod<T>
{
    Task<Result<Message>> CreateMessage(T messageData);
}
