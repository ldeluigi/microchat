using ChatService.Domain;
using ChatService.Domain.Aggregates.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

namespace ChatService.Application.DomainEvents;

public class PrivateChatDeletedHandler : DomainEventHandlerBase<PrivateChatDeleted>
{
    private readonly IPrivateMessageRepository _privateMessageRepository;

    public PrivateChatDeletedHandler(IPrivateMessageRepository privateMessageRepository)
    {
        _privateMessageRepository = privateMessageRepository;
    }

    protected override async Task<Response<Nothing>> Handle(PrivateChatDeleted ev)
    {
        await _privateMessageRepository.DeleteMessagesOfChat(ev.ChatId);
        return Ok;
    }
}
