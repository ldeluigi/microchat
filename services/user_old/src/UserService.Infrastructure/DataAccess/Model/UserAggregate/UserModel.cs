using EasyDesk.CleanArchitecture.Application.Data;
using EasyDesk.CleanArchitecture.Application.Mapping;
using Microchat.UserService.Application.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.DataAccess.UserAggregate;

public class UserModel
{
    public UserModel()
    {
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public class Configuration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(DataConstraints.NameLength);

            builder.Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(DataConstraints.NameLength);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(DataConstraints.EmailLength);
        }
    }

    public class MappingToUserOutput : DirectMapping<UserModel, UserOutput>
    {
        public MappingToUserOutput()
            : base(a => new UserOutput(
                a.Id,
                a.Name,
                a.Surname,
                a.Email))
        {
        }
    }
}
