using ChatService.Application;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using ChatService.Infrastructure.DataAccess.Queries.PrivateChats.Mappers;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetAllPrivateChatsOfUserHandler : RequestHandlerBase<GetAllPrivateChatsOfUser, IEnumerable<PrivateChatOfUserOutput>>
{
    private readonly ChatContext _chatContext;
    private readonly IUserInfoProvider _userInfoProvider;

    public GetAllPrivateChatsOfUserHandler(ChatContext chatContext, IUserInfoProvider userInfoProvider)
    {
        _chatContext = chatContext;
        _userInfoProvider = userInfoProvider;
    }

    protected override async Task<Response<IEnumerable<PrivateChatOfUserOutput>>> Handle(GetAllPrivateChatsOfUser request)
    {
        var userId = _userInfoProvider.RequireUserId();
        var result = await _chatContext.PrivateChats
            .Where(c => c.CreatorId == userId || c.PartecipantId == userId)
            .Select(c => PrivateChatModelMapper.ConvertModelToOutput(c, userId))
            .ToListAsync();
        return ResponseImports.Success<IEnumerable<PrivateChatOfUserOutput>>(result);
    }
}
