using EasyDesk.CleanArchitecture.Dal.EfCore.Domain;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.DataAccess.Model.UserAggregate;

namespace UserService.Infrastructure.DataAccess;

public class UserContext : DomainContext
{
    public DbSet<UserModel> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    protected override void ConfigureDomainModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);
    }
}
