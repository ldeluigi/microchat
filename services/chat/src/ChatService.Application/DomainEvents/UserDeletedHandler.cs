using ChatService.Domain;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Application.DomainEvents;

public class UserDeletedHandler : IDomainEventHandler<UserDeleted>
{
    private readonly IPrivateChatRepository _privateChatRepository;
    private readonly IPrivateMessageRepository _privateMessageRepository;

    public UserDeletedHandler(IPrivateChatRepository privateChatRepository, IPrivateMessageRepository privateMessageRepository)
    {
        _privateChatRepository = privateChatRepository;
        _privateMessageRepository = privateMessageRepository;
    }

    public async Task<Result<Nothing>> Handle(UserDeleted ev)
    {
        await _privateChatRepository.RemoveUserFromChats(ev.UserId);
        await _privateChatRepository.DeleteEmptyChats();
        await _privateMessageRepository.DeleteMessagesOfDeletedChats();
        await _privateMessageRepository.RemoveUserFromSender(ev.UserId);
        return Ok;
    }
}
