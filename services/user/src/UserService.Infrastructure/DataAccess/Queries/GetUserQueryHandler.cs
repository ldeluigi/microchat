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

namespace UserService.Infrastructure.DataAccess.Queries;

public class GetUserQueryHandler : RequestHandlerBase<GetUser.Query, UserOutput>
{
    private readonly UserContext _userContext;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(UserContext authContext, IMapper mapper)
    {
        _userContext = authContext;
        _mapper = mapper;
    }

    protected override async Task<Response<UserOutput>> Handle(GetUser.Query request)
    {
        return await _userContext
            .Users
            .Where(u => u.Id == request.UserId)
            .ProjectTo<UserOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
    }
}
