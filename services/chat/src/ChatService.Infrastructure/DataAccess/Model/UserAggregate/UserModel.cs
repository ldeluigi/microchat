using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ChatService.Infrastructure.DataAccess.Model.UserAggregate;

public class UserModel
{
    public Guid Id { get; set; }

    public Timestamp LastSeenTime { get; set; }

    public ICollection<PrivateChatModel> PrivateChatCreated { get; set; }

    public ICollection<PrivateChatModel> PrivateChatJoined { get; set; }

    public class Configuration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder
                .HasKey(x => x.Id);
        }
    }
}
