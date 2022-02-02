using EasyDesk.CleanArchitecture.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.DataAccess;

public class UserContext : DomainContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    protected override void ConfigureDomainModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);
    }
}
