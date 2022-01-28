using System;
using UserService.Domain.Aggregates.UserAggregate;

namespace Microchat.UserService.Application.Queries;

public record UserOutput
    (
    Guid Id,
    string Name,
    string Surame,
    string Email)
{
    public static UserOutput From(User user) => new(
        Id: user.Id,
        Name: user.Name,
        Surame: user.Surname,
        Email: user.Email);
}
