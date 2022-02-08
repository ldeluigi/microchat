using System;
using AuthService.Domain.Aggregates.AccountAggregate;

namespace AuthService.Application.Queries.Accounts.Outputs;

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
