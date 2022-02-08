using EasyDesk.CleanArchitecture.Domain.Metamodel.Repositories;
using System;
using System.Threading.Tasks;

namespace ChatService.Domain.Aggregates.MessageAggregate;
public interface IPrivateMessageRepository :
    IGetByIdRepository<PrivateMessage, Guid>,
    ISaveRepository<PrivateMessage>,
    IRemoveRepository<PrivateMessage>
{
    Task DeleteMessagesOfChat(Guid chatId);

    Task DeleteMessagesOfDeletedChats();

    Task RemoveUserFromSender(Guid userId);
}
