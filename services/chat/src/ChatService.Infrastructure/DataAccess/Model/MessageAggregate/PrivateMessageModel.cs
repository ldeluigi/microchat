using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.UserAggregate;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ChatService.Infrastructure.DataAccess.Model.MessageAggregate;

public class PrivateMessageModel
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public string Text { get; set; }

    public Guid? SenderId { get; set; }

    public Timestamp SendTime { get; set; }

    public Timestamp LastEditTime { get; set; }

    public bool Viewed { get; set; }

    public UserModel Sender { get; set; }

    public PrivateChatModel Chat { get; set; }

    public class Configuration : IEntityTypeConfiguration<PrivateMessageModel>
    {
        public void Configure(EntityTypeBuilder<PrivateMessageModel> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(false);

            builder.HasIndex(x => x.SendTime)
                .IsClustered();

            builder.HasIndex(x => x.ChatId);

            builder.HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(x => x.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
