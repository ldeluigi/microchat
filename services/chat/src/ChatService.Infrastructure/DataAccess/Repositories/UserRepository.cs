using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.UserAggregate;
using ChatService.Infrastructure.DataAccess.Model.UserAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Dal.EfCore.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Repositories;

public class UserRepository : EfCoreRepository<User, UserModel, ChatContext>, IUserRepository
{
    public UserRepository(
        ChatContext chatContext,
        IModelConverter<User, UserModel> modelConverter,
        IDomainEventNotifier domainEventNotifier)
        : base(chatContext, modelConverter, domainEventNotifier)
    {
    }

    public async Task<Result<User>> GetById(Guid id) =>
        await GetSingle(q => q.Where(u => u.Id == id));

    protected override DbSet<UserModel> GetDbSet(ChatContext context) => context.Users;

    protected override IQueryable<UserModel> Includes(IQueryable<UserModel> initialQuery) =>
        initialQuery;
}
