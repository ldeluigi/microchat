using ChatService.Domain.Aggregates.ChatAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Domain.ChatService;

public record PrivateChatRegistrationData(Guid Id, Guid Owner, Guid Partecipant, Timestamp CreationTime);

public class PrivateChatRegistrationMethod : IRegistrationMethod<PrivateChatRegistrationData>
{
    private readonly IPrivateChatRepository _privateChatRepository;

    public PrivateChatRegistrationMethod(IPrivateChatRepository privateChatRepository)
    {
        _privateChatRepository = privateChatRepository;
    }

    /// <inheritdoc/>
    public Task<Result<PrivateChat>> CreatePrivateChat(PrivateChatRegistrationData chatData)
    {
        var chat = PrivateChat.Create(
            chatData.Id,
            chatData.Owner,
            chatData.Partecipant,
            chatData.CreationTime);

        _privateChatRepository.Save(chat);

        return Task.FromResult(Success(chat));
    }
}
