using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using ChatService.Domain.Aggregates.UserAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;

namespace ChatService.Domain;

public class UserLifecycleService
{
    public UserLifecycleService(
        IUserRepository userRepository,
        IPrivateChatRepository privateChatRepository,
        IPrivateMessageRepository privateMessageRepository)
    {

    }

    public Task<Result<Nothing>> DeleteUser(Guid userId)
    {
        // TODO: How to implement?
        throw new NotImplementedException();
    }
}
