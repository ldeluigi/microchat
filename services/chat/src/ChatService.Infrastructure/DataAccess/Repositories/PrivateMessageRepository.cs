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

namespace ChatService.Infrastructure.DataAccess.Repositories;

public class PrivateMessageRepository : EfCoreRepository<PrivateMessage, PrivateMessageModel, ChatContext>, IPrivateMessageRepository
{
    public PrivateMessageRepository(
        ChatContext context,
        IModelConverter<PrivateMessage, PrivateMessageModel> converter,
        IDomainEventNotifier eventNotifier)
        : base(context, converter, eventNotifier)
    {
    }

    public async Task<Result<PrivateMessage>> GetById(Guid id) => await GetSingle(q => q.Where(q => q.Id == id));

    protected override DbSet<PrivateMessageModel> GetDbSet(ChatContext context) => context.PrivateMessages;

    protected override IQueryable<PrivateMessageModel> Includes(IQueryable<PrivateMessageModel> initialQuery) => initialQuery;
}
