using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using Microchat.UserService.Application.Queries;
using System.Linq;
using System.Threading.Tasks;
using UserService.Application;

namespace UserService.Infrastructure.DataAccess.Queries;

public class GetUserQueryHandler : RequestHandlerBase<GetUser.Query, UserOutput>
{
    private readonly UserContext _userContext;
    private readonly IMapper _mapper;
    private readonly IUserInfoProvider _userInfoProvider;

    public GetUserQueryHandler(UserContext userContext, IMapper mapper, IUserInfoProvider userInfoProvider)
    {
        _userContext = userContext;
        _mapper = mapper;
        _userInfoProvider = userInfoProvider;
    }

    protected override async Task<Response<UserOutput>> Handle(GetUser.Query request)
    {
        if (_userInfoProvider.RequireUserId() != request.UserId)
        {
            return ResponseImports.Failure<UserOutput>(new NotFoundError());
        }
        return await _userContext
            .Users
            .Where(u => u.Id == request.UserId)
            .ProjectTo<UserOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
    }
}
