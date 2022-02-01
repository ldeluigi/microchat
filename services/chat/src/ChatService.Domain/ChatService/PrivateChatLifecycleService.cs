using ChatService.Domain.Aggregates.ChatAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Threading.Tasks;

namespace ChatService.Domain.ChatService;

public class PrivateChatLifecycleService
{
    private readonly IPrivateChatRepository _chatRepository;
    private readonly IRegistrationMethod<PrivateChatRegistrationData> _registrationMethod;

    public PrivateChatLifecycleService(
        IPrivateChatRepository chatRepository,
        PrivateChatRegistrationMethod privateChatRegistrationMethod)
    {
        _chatRepository = chatRepository;
        _registrationMethod = privateChatRegistrationMethod;
    }

    public async Task<Result<PrivateChat>> Register(Guid id, Guid owner, Guid partecipant, Timestamp creationTime)
    {
        return await Task.FromResult(ResultImports.Ok)
            .ThenFlatMapAsync(_ => _registrationMethod.CreatePrivateChat(
                new PrivateChatRegistrationData(id, owner, partecipant, creationTime))
            .ThenIfSuccess(chat => _chatRepository.Save(chat)));
    }

    public async Task<Result<PrivateChat>> Unregister(Guid chatId)
    {
        return await _chatRepository
            .GetById(chatId)
            .ThenIfSuccess(chat => _chatRepository.Remove(chat));
    }
}
