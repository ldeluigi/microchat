using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;

namespace AuthService.Application.Queries.Accounts;

public static class GetAccounts
{
    public record Query(string SearchString, Pagination Pagination) : PaginatedQueryBase<AccountOutput>(Pagination);
}
