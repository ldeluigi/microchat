using EasyDesk.Tools.Options;
using System;
using UserService.Domain.Aggregates.UserAggregate;

namespace Microchat.UserService.Application.Queries;

public record UserOutput
    (Guid Id,
    string Name,
    string Surname,
    string Username)
{
    public static UserOutput From(User user) => new(
        Id: user.Id,
        Name: user.Name.Select(n => n.Value).OrElse(string.Empty),
        Surname: user.Surname.Select(n => n.Value).OrElse(string.Empty),
        Username: user.Username);
}
