using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator.Handlers;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using Microchat.UserService.Application.Queries;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UserService.Infrastructure.DataAccess.Queries;

public class GetUserQueryHandler : IQueryHandler<GetUser.Query, UserOutput>
{
    private readonly UserContext _userContext;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(UserContext userContext, IMapper mapper)
    {
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<Response<UserOutput>> Handle(GetUser.Query request, CancellationToken cancellationToken)
    {
        return await _userContext
            .Users
            .Where(u => u.Id == request.UserId)
            .ProjectTo<UserOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
    }
}
