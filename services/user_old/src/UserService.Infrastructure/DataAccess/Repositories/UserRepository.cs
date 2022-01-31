using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Dal.EfCore.Repositories;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Infrastructure.DataAccess.UserAggregate;

namespace UserService.Infrastructure.DataAccess.Repositories;

public class UserRepository : EfCoreRepository<User, UserModel, UserContext>, IUserRepository
{
    public UserRepository(
        UserContext context,
        IModelConverter<User, UserModel> converter,
        IDomainEventNotifier eventNotifier) : base(context, converter, eventNotifier)
    {
    }

    public async Task<Result<User>> GetByEmail(Email email) =>
        await GetSingle(q => q.Where(a => a.Email == email.ToString()));

    public async Task<Result<User>> GetById(Guid id) =>
        await GetSingle(q => q.Where(a => a.Id == id));

    protected override DbSet<UserModel> GetDbSet(UserContext context) =>
        context.Users;

    protected override IQueryable<UserModel> Includes(IQueryable<UserModel> initialQuery) =>
        initialQuery;
}
