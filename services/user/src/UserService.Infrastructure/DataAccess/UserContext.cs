using EasyDesk.CleanArchitecture.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserService.Infrastructure.DataAccess.UserAggregate;

namespace UserService.Infrastructure.DataAccess;

public class UserContext : EntitiesContext
{
    public DbSet<UserModel> Users { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContext"/> class.
    /// </summary>
    /// <param name="options">db options.</param>
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
