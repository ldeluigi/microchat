using System;
using System.Collections.Generic;
using AuthService.Application.Queries.Accounts;
using EasyDesk.CleanArchitecture.Application.Mapping;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

    public string Password { get; set; }

    public string Salt { get; set; }

    public virtual ICollection<SessionModel> Sessions { get; set; }

    public class Configuration : IEntityTypeConfiguration<AccountModel>
    {
        public void Configure(EntityTypeBuilder<AccountModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(s => s.Password)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(s => s.Salt)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Creation)
                .IsRequired();

            builder.Property(x => x.Username)
                .HasMaxLength(50);

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.HasMany(x => x.Sessions)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MappingToAccountOutput : DirectMapping<AccountModel, AccountOutput>
    {
        public MappingToAccountOutput()
            : base(a => new AccountOutput(
                a.Id,
                a.Email,
                a.Username))
        {
        }
    }
}
