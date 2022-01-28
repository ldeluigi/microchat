using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.CleanArchitecture.Domain.Utils;
using Microchat.UserService.Application.Queries;
using System.Linq;
using System.Threading.Tasks;
using UserService.Infrastructure.DataAccess;

namespace UserService.Infrastructure.DataAccess.Queries;

public class GetUserQueryHandler : RequestHandlerBase<GetUser.Query, UserOutput>
{
    private readonly UserContext _authContext;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(UserContext authContext, IMapper mapper)
    {
        _authContext = authContext;
        _mapper = mapper;
    }

    protected override async Task<Response<UserOutput>> Handle(GetUser.Query request)
    {
        return await _authContext
            .Users
            .Where(u => u.Id == request.UserId)
            .ProjectTo<UserOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
    }
}
