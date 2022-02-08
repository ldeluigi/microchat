using ChatService.Domain;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools;
using System.Threading.Tasks;
using static EasyDesk.CleanArchitecture.Application.Responses.ResponseImports;

namespace ChatService.Application.DomainEvents;

public class UserDeletedHandler : DomainEventHandlerBase<UserDeleted>
{
    private readonly IPrivateChatRepository _privateChatRepository;
    private readonly IPrivateMessageRepository _privateMessageRepository;

    public UserDeletedHandler(IPrivateChatRepository privateChatRepository, IPrivateMessageRepository privateMessageRepository)
    {
        _privateChatRepository = privateChatRepository;
        _privateMessageRepository = privateMessageRepository;
    }

    protected override async Task<Response<Nothing>> Handle(UserDeleted ev)
    {
        await _privateChatRepository.RemoveUserFromChats(ev.UserId);
        await _privateChatRepository.DeleteEmptyChats();
        await _privateMessageRepository.DeleteMessagesOfDeletedChats();
        await _privateMessageRepository.RemoveUserFromSender(ev.UserId);
        return Ok;
    }
}
