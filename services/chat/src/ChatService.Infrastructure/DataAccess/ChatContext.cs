using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using ChatService.Infrastructure.DataAccess.Model.UserAggregate;
using EasyDesk.CleanArchitecture.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure.DataAccess;

public class ChatContext : DomainContext
{
    public DbSet<PrivateChatModel> PrivateChats { get; set; }

    public DbSet<PrivateMessageModel> PrivateMessages { get; set; }

    public DbSet<UserModel> Users { get; set; }

    public ChatContext(DbContextOptions<ChatContext> options) : base(options)
    {
    }

    protected override void ConfigureDomainModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatContext).Assembly);
    }
}
