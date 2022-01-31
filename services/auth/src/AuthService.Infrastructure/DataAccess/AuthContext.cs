using AuthService.Infrastructure.DataAccess.Model.AccountAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.DataAccess;

public class AuthContext : EntitiesContext
{
    public DbSet<AccountModel> Accounts { get; set; }

    public DbSet<SessionModel> Sessions { get; set; }

    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }

    protected override void SetupModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthContext).Assembly);
    }
}
