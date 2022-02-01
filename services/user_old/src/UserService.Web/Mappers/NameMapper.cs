using EasyDesk.CleanArchitecture.Application.Mapping;
using UserService.Domain.Aggregates.UserAggregate;

namespace UserService.Web.Mappers;

public class NameMapper : DirectMapping<string, Name>
{
    public NameMapper() : base(s => Name.From(s))
    {
    }
}
