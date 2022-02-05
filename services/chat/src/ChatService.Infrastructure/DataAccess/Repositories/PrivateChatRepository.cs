using ChatService.Domain.Aggregates.PrivateChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Dal.EfCore.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Repositories;

public class PrivateChatRepository : EfCoreRepository<PrivateChat, PrivateChatModel, ChatContext>, IPrivateChatRepository
{
    public PrivateChatRepository(
        ChatContext context,
        IModelConverter<PrivateChat, PrivateChatModel> modelConverter,
        IDomainEventNotifier domainEventNotifier)
        : base(context, modelConverter, domainEventNotifier)
    {
    }

    public async Task<bool> ChatAlreadyExistBetween(Guid user1, Guid user2) =>
        await DbSet.Where(c =>
            (c.PartecipantId == user1 && c.CreatorId == user2)
            || (c.PartecipantId == user2 && c.CreatorId == user1))
        .AnyAsync();

    public void DeleteOrphanChats()
    {
        var orphanChats = DbSet.Where(c => c.PartecipantId == null && c.CreatorId == null);
        DbSet.RemoveRange(orphanChats);
    }

    public async Task<Result<PrivateChat>> GetById(Guid id) => await GetSingle(q => q.Where(x => x.Id == id));

    protected override DbSet<PrivateChatModel> GetDbSet(ChatContext context) => context.PrivateChats;

    protected override IQueryable<PrivateChatModel> Includes(IQueryable<PrivateChatModel> initialQuery) =>
        initialQuery;
}
