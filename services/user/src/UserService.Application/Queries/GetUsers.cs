using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using FluentValidation;
using Microchat.UserService.Application.Queries;

namespace UserService.Application.Queries;

public static class GetUsers
{
    public record Query(
        string SearchString,
        Pagination Pagination) : QueryWithPaginationBase<UserOutput>(Pagination);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.SearchString).NotEmpty();
        }
    }
}
