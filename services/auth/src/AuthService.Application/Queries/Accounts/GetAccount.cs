using EasyDesk.CleanArchitecture.Application.Mediator;
using System;

namespace AuthService.Application.Queries.Accounts;

public static class GetAccount
{
    public record Query(Guid UserId) : QueryBase<AccountOutput>;
}
