using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AuthService.Infrastructure.DataAccess.Model.AccountAggregate;

public class SessionModel
{
    public string RefreshToken { get; set; }

    public Timestamp Expiration { get; set; }

    public Guid AccessTokenId { get; set; }

    public Guid AccountId { get; set; }

    public AccountModel Account { get; set; }

    public class Configuration : IEntityTypeConfiguration<SessionModel>
    {
        public void Configure(EntityTypeBuilder<SessionModel> builder)
        {
            builder.HasKey(x => x.RefreshToken);

            builder.Property(x => x.RefreshToken)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Expiration)
                .IsRequired();
        }
    }
}
