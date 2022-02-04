using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using ChatService.Domain.Aggregates.UserAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Domain;

public class UserLifecycleService
{
    private readonly IUserRepository _userRepository;
    private readonly IPrivateChatRepository _privateChatRepository;

    public UserLifecycleService(
        IUserRepository userRepository,
        IPrivateChatRepository privateChatRepository)
    {
        _userRepository = userRepository;
        _privateChatRepository = privateChatRepository;
    }

    public async Task<Result<Nothing>> DeleteUser(Guid userId)
    {
        await _userRepository.RemoveById(userId);
        _privateChatRepository.DeleteOrphanChats();
        return Ok;
    }

    public Task<Result<User>> CreateUser(Guid accountId)
    {
        var user = User.Create(accountId);
        _userRepository.Save(user);
        return Task.FromResult(Success(user));
    }
}
