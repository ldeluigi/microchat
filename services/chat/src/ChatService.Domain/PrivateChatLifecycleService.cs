using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.Tools;
using System;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Domain;

public record PrivateChatAlreadyExistsBetween(Guid User1, Guid User2) : DomainError;

public record PrivateChatDeleted(Guid ChatId) : DomainEvent;

public class PrivateChatLifecycleService
{
    private readonly IPrivateChatRepository _privateChatRepository;
    private readonly IDomainEventNotifier _domainEventNotifier;
    private readonly ITimestampProvider _timestampProvider;

    public PrivateChatLifecycleService(
        IPrivateChatRepository privateChatRepository,
        IDomainEventNotifier domainEventNotifier,
        ITimestampProvider timestampProvider)
    {
        _privateChatRepository = privateChatRepository;
        _domainEventNotifier = domainEventNotifier;
        _timestampProvider = timestampProvider;
    }

    public async Task<Result<PrivateChat>> CreatePrivateChat(Guid creatorId, Guid participantId)
    {
        return await _privateChatRepository.ChatAlreadyExistBetween(creatorId, participantId)
            .Map(alreadyExists =>
            {
                if (alreadyExists)
                {
                    return new PrivateChatAlreadyExistsBetween(creatorId, participantId);
                }
                var chat = PrivateChat.Create(
                Guid.NewGuid(),
                creatorId,
                participantId,
                _timestampProvider.Now);
                _privateChatRepository.Save(chat);
                return Success(chat);
            });
    }

    public async Task<Result<PrivateChat>> DeletePrivateChat(Guid id)
    {
        return await _privateChatRepository
                .GetById(id)
                .ThenIfSuccess(chat => _privateChatRepository.Remove(chat))
                .ThenIfSuccess(chat => _domainEventNotifier.Notify(new PrivateChatDeleted(chat.Id)));
    }
}
