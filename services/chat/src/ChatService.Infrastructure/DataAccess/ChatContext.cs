using EasyDesk.CleanArchitecture.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure.DataAccess;

public class ChatContext : DomainContext
{
    public ChatContext(DbContextOptions<ChatContext> options) : base(options)
    {
    }

    protected override void ConfigureDomainModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatContext).Assembly);
    }
}
