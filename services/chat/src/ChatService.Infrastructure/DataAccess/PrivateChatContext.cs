using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure.DataAccess;

public class PrivateChatContext : EntitiesContext
{
    public DbSet<PrivateChatModel> PrivateChats { get; set; }

    public PrivateChatContext(DbContextOptions<PrivateChatContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrivateChatContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
