using EasyDesk.CleanArchitecture.Application.Messaging;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using System;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Application.Messages;

public record UsernameChanged(Guid AccountId, string Username) : IMessage;

public class UsernameUpdateHandler : IMessageHandler<UsernameChanged>
{
    private readonly IUserRepository _userRepository;

    public UsernameUpdateHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UsernameChanged message)
    {
        await _userRepository.GetById(message.AccountId)
            .ThenIfSuccess(user =>
                user.UpdateUsername(Username.From(message.Username)));
    }
}
