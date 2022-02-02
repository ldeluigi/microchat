using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Dal.EfCore.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Infrastructure.DataAccess.Model.UserAggregate;

namespace UserService.Infrastructure.DataAccess.Repositories;

public class UserRepository : EfCoreRepository<User, UserModel, UserContext>, IUserRepository
{
    public UserRepository(
        UserContext context,
        IModelConverter<User, UserModel> converter,
        IDomainEventNotifier eventNotifier) : base(context, converter, eventNotifier)
    {
    }

    public async Task<Result<User>> GetById(Guid id) =>
        await GetSingle(q => q.Where(x => x.Id == id));

    protected override DbSet<UserModel> GetDbSet(UserContext context) => context.Users;

    protected override IQueryable<UserModel> Includes(IQueryable<UserModel> initialQuery) => initialQuery;
}
