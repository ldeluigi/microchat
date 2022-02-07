using ChatService.Application;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Infrastructure.DataAccess.Queries.PrivateChats.Mappers;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.Tools.Options;
using System.Linq;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatsOfUserHandler : PaginatedQueryHandlerBase<GetPrivateChatsOfUser, PrivateChatOfUserOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IUserInfoProvider _userInfoProvider;

    public GetPrivateChatsOfUserHandler(ChatContext chatContext, IUserInfoProvider userInfoProvider)
    {
        _chatContext = chatContext;
        _userInfoProvider = userInfoProvider;
    }

    protected override async Task<Response<Page<PrivateChatOfUserOutput>>> Handle(GetPrivateChatsOfUser request) =>
        await _chatContext.PrivateChats
            .Where(c => c.CreatorId == request.UserId || c.PartecipantId == request.UserId)
            .OrderBy(c => c.Id)
            .Select(c => PrivateChatModelMapper.ConvertModelToOutput(c, _userInfoProvider.RequireUserId()))
            .GetPageAsync(request.Pagination);
}
