using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ChatService.Infrastructure.DataAccess.Model.ChatAggregate;

public class PrivateChatModel
{
    public Guid Id { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? PartecipantId { get; set; }

    public Timestamp CreationTime { get; set; }

    public class Configuration : IEntityTypeConfiguration<PrivateChatModel>
    {
        public void Configure(EntityTypeBuilder<PrivateChatModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasIndex(chat => new { chat.CreatorId, chat.PartecipantId })
                .IsUnique();

            builder
                .HasIndex(chat => new { chat.PartecipantId, chat.CreatorId })
                .IsUnique();
        }
    }
}
