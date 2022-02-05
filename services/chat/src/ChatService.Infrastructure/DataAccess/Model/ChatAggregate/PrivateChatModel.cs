﻿using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using ChatService.Infrastructure.DataAccess.Model.UserAggregate;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ChatService.Infrastructure.DataAccess.Model.ChatAggregate;

public class PrivateChatModel
{
    public Guid Id { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? PartecipantId { get; set; }

    public Timestamp CreationTime { get; set; }

    public UserModel Creator { get; set; }

    public UserModel Participant { get; set; }

    public ICollection<PrivateMessageModel> Messages { get; set; }

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

            builder.HasOne(x => x.Creator)
                .WithMany(u => u.PrivateChatCreated)
                .HasForeignKey(u => u.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Participant)
                .WithMany(u => u.PrivateChatJoined)
                .HasForeignKey(x => x.PartecipantId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
