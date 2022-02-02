using AutoMapper;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using Microchat.UserService.Application.Queries;
using System.Threading.Tasks;
using UserService.Application.Queries;

namespace UserService.Infrastructure.DataAccess.Queries;

public class GetUsersQueryHandler : PaginatedQueryHandlerBase<GetUsers.Query, UserOutput>
{
    private readonly UserContext _userContext;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(UserContext userContext, IMapper mapper)
    {
        _userContext = userContext;
        _mapper = mapper;
    }

    protected override async Task<Response<Page<UserOutput>>> Handle(GetUsers.Query request)
    {
        // TODO: implement
        throw new System.NotImplementedException();
    }
}
