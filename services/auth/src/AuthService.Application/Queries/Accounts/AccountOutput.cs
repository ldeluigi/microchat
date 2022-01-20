using AuthService.Domain.Aggregates.AccountAggregate;
using System;

namespace AuthService.Application.Queries.Accounts;

public record AccountOutput(
    Guid Id,
    string Email,
    string Username)
{
    public static AccountOutput From(Account account) => new(
        Id: account.Id,
        Email: account.Email,
        Username: account.Username);
}
