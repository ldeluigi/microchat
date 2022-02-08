using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Dal.EfCore.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ChatService.Infrastructure.DataAccess.Repositories;

public class PrivateMessageRepository : EfCoreRepository<PrivateMessage, PrivateMessageModel, ChatContext>, IPrivateMessageRepository
{
    private readonly ChatContext _context;

    public PrivateMessageRepository(
        ChatContext context,
        IModelConverter<PrivateMessage, PrivateMessageModel> converter,
        IDomainEventNotifier eventNotifier)
        : base(context, converter, eventNotifier)
    {
        _context = context;
    }

    public Task DeleteMessagesOfChat(Guid chatId) =>
        DbSet.Where(c => c.ChatId == chatId)
        .DeleteAsync();

    public Task DeleteMessagesOfDeletedChats() =>
        DbSet.GroupJoin(
                _context.PrivateChats,
                on => on.ChatId,
                on => on.Id,
                (m, c) => new { Message = m, ToDelete = c.Count() == 0 })
            .Where(x => x.ToDelete)
            .Select(x => x.Message)
            .DeleteAsync();

    public async Task<Result<PrivateMessage>> GetById(Guid id) => await GetSingle(q => q.Where(q => q.Id == id));

    public Task RemoveUserFromSender(Guid userId) =>
        DbSet.Where(m => m.SenderId == userId)
        .UpdateAsync(m => new() { SenderId = null });

    protected override DbSet<PrivateMessageModel> GetDbSet(ChatContext context) => context.PrivateMessages;

    protected override IQueryable<PrivateMessageModel> Includes(IQueryable<PrivateMessageModel> initialQuery) => initialQuery;
}
