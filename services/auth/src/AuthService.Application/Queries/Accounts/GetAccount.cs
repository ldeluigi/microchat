using System;
using AuthService.Application.Queries.Accounts.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;

namespace AuthService.Application.Queries.Accounts;

public static class GetAccount
{
    public record Query(Guid AccountId) : QueryBase<AccountOutput>;
}
