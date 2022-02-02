using System;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Infrastructure.DataAccess.Model.AccountAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Dal.EfCore.Repositories;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.CleanArchitecture.Domain.Metamodel;
using EasyDesk.CleanArchitecture.Domain.Metamodel.Results;
using EasyDesk.CleanArchitecture.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.DataAccess.Repositories;

public class AccountRepository : EfCoreRepository<Account, AccountModel, AuthContext>, IAccountRepository
{
    public AccountRepository(
        AuthContext context,
        IModelConverter<Account, AccountModel> converter,
        IDomainEventNotifier eventNotifier) : base(context, converter, eventNotifier)
    {
    }

    public async Task<bool> EmailExists(Email email) =>
        await DbSet.AnyAsync(a => a.Email == email.ToString());

    public async Task<(bool EmailExists, bool UsernameExists)> EmailOrUsernameExists(Email email, Username username)
    {
        var user = await DbSet
            .AsNoTracking()
            .Where(a => a.Email == email.ToString() || a.Username == username.ToString())
            .FirstOptionAsync();

        return user.Match(
            some: a => (a.Email == email.ToString(), a.Username == username.ToString()),
            none: () => (false, false));
    }

    public async Task<bool> UsernameExists(Username username) =>
        await DbSet.AnyAsync(a => a.Username == username.ToString());

    public async Task<Result<Account>> GetByEmail(Email email) =>
        await GetSingle(q => q.Where(a => a.Email == email.ToString()));

    public async Task<Result<Account>> GetById(Guid id) =>
        await GetSingle(q => q.Where(a => a.Id == id));

    public async Task<Result<Account>> GetByRefreshToken(Token token) =>
        await GetSingle(q => q.Where(u => u.Sessions.Any(s => s.RefreshToken == token.ToString())));

    public async Task<Result<Account>> GetByUsername(Username username) =>
        await GetSingle(q => q.Where(a => a.Username == username.ToString()));

    protected override DbSet<AccountModel> GetDbSet(AuthContext context) =>
        context.Accounts;

    protected override IQueryable<AccountModel> Includes(IQueryable<AccountModel> initialQuery) =>
        initialQuery
        .Include(a => a.Sessions);
}
