using ChatService.Domain;
using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.Tools;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Domain.Metamodel.Results.ResultImports;

namespace ChatService.Application.DomainEvents;

public class PrivateChatDeletedHandler : IDomainEventHandler<PrivateChatDeleted>
{
    private readonly IPrivateMessageRepository _privateMessageRepository;

    public PrivateChatDeletedHandler(IPrivateMessageRepository privateMessageRepository)
    {
        _privateMessageRepository = privateMessageRepository;
    }

    public async Task<Result<Nothing>> Handle(PrivateChatDeleted ev)
    {
        await _privateMessageRepository.DeleteMessagesOfChat(ev.ChatId);
        return Ok;
    }
}
