using AuthService.Application.Queries.Accounts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.CleanArchitecture.Domain.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.DataAccess.Queries;

public class GetAccountQueryHandler : RequestHandlerBase<GetAccount.Query, AccountOutput>
{
    private readonly AuthContext _authContext;
    private readonly IMapper _mapper;

    public GetAccountQueryHandler(AuthContext authContext, IMapper mapper)
    {
        _authContext = authContext;
        _mapper = mapper;
    }

    protected override async Task<Response<AccountOutput>> Handle(GetAccount.Query request)
    {
        return await _authContext
            .Accounts
            .Where(u => u.Id == request.UserId)
            .ProjectTo<AccountOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
    }
}
