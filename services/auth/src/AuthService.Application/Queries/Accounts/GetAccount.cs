using System;
using EasyDesk.CleanArchitecture.Application.Mediator;

namespace AuthService.Application.Queries.Accounts;

public static class GetAccount
{
    public record Query(Guid UserId) : QueryBase<AccountOutput>;
}
