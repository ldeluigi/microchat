using ChatService.Domain.Aggregates.ChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Dal.EfCore.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Repositories
{
    public class PrivateChatRepositories : EfCoreRepository<PrivateChat, PrivateChatModel, PrivateChatContext>, IPrivateChatRepository
    {
        public PrivateChatRepositories(
            PrivateChatContext context,
            IModelConverter<PrivateChat, PrivateChatModel> converter,
            IDomainEventNotifier eventNotifier) : base(context, converter, eventNotifier)
        {
        }

        public async Task<Result<PrivateChat>> GetById(Guid id) =>
            await GetSingle(q => q.Where(a => a.Id == id));

        protected override DbSet<PrivateChatModel> GetDbSet(PrivateChatContext context) =>
            context.PrivateChats;

        protected override IQueryable<PrivateChatModel> Includes(IQueryable<PrivateChatModel> initialQuery) =>
            initialQuery;
    }

}
