using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using Microchat.UserService.Application.Queries;

namespace UserService.Application.Queries;

public static class GetUsers
{
    public record Query(string SearchString, Pagination Pagination) : PaginatedQueryBase<UserOutput>(Pagination);
}
