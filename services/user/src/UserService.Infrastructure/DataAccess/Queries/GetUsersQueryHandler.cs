using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using Microchat.UserService.Application.Queries;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Application.Queries;

namespace UserService.Infrastructure.DataAccess.Queries;

public class GetUsersQueryHandler : PaginatedQueryHandlerBase<GetUsers.Query, UserOutput>
{
    private static readonly StringComparison _caseInsensitive = StringComparison.InvariantCultureIgnoreCase;

    private readonly UserContext _userContext;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(UserContext userContext, IMapper mapper)
    {
        _userContext = userContext;
        _mapper = mapper;
    }

    protected override async Task<Response<Page<UserOutput>>> Handle(GetUsers.Query request)
    {
        var first = _userContext
            .Users
            .Where(u => u.Username.StartsWith(request.SearchString, _caseInsensitive))
            .OrderByDescending(u => u.Username);
        var second = _userContext
            .Users
            .Where(u => u.Name.StartsWith(request.SearchString, _caseInsensitive))
            .OrderByDescending(u => u.Name);
        var third = _userContext
            .Users
            .Where(u => u.Surname.StartsWith(request.SearchString, _caseInsensitive))
            .OrderByDescending(u => u.Surname);
        return await first
            .Concat(second)
            .Concat(third)
            .ProjectTo<UserOutput>(_mapper.ConfigurationProvider)
            .GetPage(request.Pagination);
    }
}
