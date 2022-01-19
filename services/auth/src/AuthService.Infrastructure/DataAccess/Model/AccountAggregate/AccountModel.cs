using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace AuthService.Infrastructure.DataAccess.Model.AccountAggregate;

public class AccountModel
{
    public AccountModel()
    {
        Sessions = new HashSet<SessionModel>();
    }

    public Guid Id { get; set; }

    public string Username { get; set; }

    public Timestamp Creation { get; set; }

    public string Email { get; set; }

    public string EmailUpdate { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public string PasswordRecoveryToken { get; set; }

    public Timestamp PasswordRecoveryTokenExpiration { get; set; }

    public string ConfirmationToken { get; set; }

    public Timestamp ConfirmationTokenExpiration { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<SessionModel> Sessions { get; set; }

    public class Configuration : IEntityTypeConfiguration<AccountModel>
    {
        public void Configure(EntityTypeBuilder<AccountModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PasswordRecoveryToken)
                .HasMaxLength(100);

            builder.Property(s => s.Password)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(s => s.Salt)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.ConfirmationToken)
                .HasMaxLength(100);

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(DataConstraints.NameLength);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(DataConstraints.EmailLength);

            builder.Property(x => x.EmailUpdate)
                .HasMaxLength(DataConstraints.EmailLength);

            builder.Property(x => x.Creation)
                .IsRequired();

            builder.HasMany(x => x.Sessions)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
