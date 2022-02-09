using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Application;
using AuthService.Application.Queries.Accounts;
using AuthService.Application.Queries.Accounts.Outputs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator.Handlers;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.CleanArchitecture.Domain.Utils;

namespace AuthService.Infrastructure.DataAccess.Queries;

public class GetAccountQueryHandler : IQueryHandler<GetAccount.Query, AccountOutput>
{
    private readonly AuthContext _authContext;
    private readonly IMapper _mapper;
    private readonly IUserInfoProvider _userInfoProvider;

    public GetAccountQueryHandler(AuthContext authContext, IMapper mapper, IUserInfoProvider userInfoProvider)
    {
        _authContext = authContext;
        _mapper = mapper;
        _userInfoProvider = userInfoProvider;
    }

    public async Task<Response<AccountOutput>> Handle(GetAccount.Query request, CancellationToken cancellationToken)
    {
        if (_userInfoProvider.RequireUserId() != request.AccountId)
        {
            return ResponseImports.Failure<AccountOutput>(new NotFoundError());
        }
        return await _authContext
            .Accounts
            .Where(u => u.Id == request.AccountId)
            .ProjectTo<AccountOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
    }
}
