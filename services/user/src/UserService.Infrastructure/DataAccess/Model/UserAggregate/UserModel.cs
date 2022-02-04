using EasyDesk.CleanArchitecture.Application.Mapping;
using Microchat.UserService.Application.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using DomainName = UserService.Domain.Aggregates.UserAggregate.Name;

namespace UserService.Infrastructure.DataAccess.Model.UserAggregate;

public class UserModel
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public class Configuration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.HasIndex(x => x.Name);

            builder.HasIndex(x => x.Surname);

            builder.Property(x => x.Username)
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .HasMaxLength(DomainName.MaximumLength);

            builder.Property(x => x.Surname)
                .HasMaxLength(DomainName.MaximumLength);
        }
    }

    public class MappingToUserOutput : DirectMapping<UserModel, UserOutput>
    {
        public MappingToUserOutput()
            : base(u => new UserOutput(
                u.Id,
                u.Name,
                u.Surname,
                u.Username))
        {
        }
    }
}
