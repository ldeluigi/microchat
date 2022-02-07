using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ChatService.Infrastructure.DataAccess.Model.UserAggregate;

public class UserModel
{
    public Guid Id { get; set; }

    public class Configuration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder
                .HasKey(x => x.Id);
        }
    }
}
