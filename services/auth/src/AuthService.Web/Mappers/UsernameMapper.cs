using AuthService.Domain.Aggregates.AccountAggregate;
using EasyDesk.CleanArchitecture.Application.Mapping;

namespace AuthService.Web.Mappers;

public class UsernameMapper : DirectMapping<string, Username>
{
    public UsernameMapper() : base(s => Username.From(s))
    {
    }
}
