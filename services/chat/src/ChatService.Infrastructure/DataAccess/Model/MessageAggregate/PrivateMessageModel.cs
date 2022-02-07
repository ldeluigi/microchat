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

    public class Configuration : IEntityTypeConfiguration<PrivateMessageModel>
    {
        public void Configure(EntityTypeBuilder<PrivateMessageModel> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(false);

            builder.HasIndex(x => x.SendTime)
                .IsClustered();

            builder.HasIndex(x => x.ChatId);
        }
    }
}
