using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure.DataAccess.Repositories;

public class MessageContext : EntitiesContext
{
    public DbSet<MessageModel> Messages { get; set; }

    public MessageContext(DbContextOptions<PrivateChatContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessageContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
