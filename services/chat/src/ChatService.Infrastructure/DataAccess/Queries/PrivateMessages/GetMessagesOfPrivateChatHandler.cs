using AutoMapper;
using ChatService.Application;
using ChatService.Application.Queries.PrivateMessages;
using ChatService.Application.Queries.PrivateMessages.Outputs;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Mediator.Handlers;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using EasyDesk.Tools.Options;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateMessages;

public class GetMessagesOfPrivateChatHandler : IQueryWithPaginationHandler<GetMessagesOfPrivateChat, PrivateChatMessageOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IMapper _mapper;

    public GetMessagesOfPrivateChatHandler(ChatContext chatContext, IUserInfoProvider userInfoProvider, IMapper mapper)
    {
        _chatContext = chatContext;
        _userInfoProvider = userInfoProvider;
        _mapper = mapper;
    }

    private static PrivateChatMessageOutput ConvertModelToOutput(PrivateMessageModel privateMessage, PrivateChatModel chatModel) => new(
        Id: privateMessage.Id,
        ChatId: privateMessage.ChatId,
        SendTime: privateMessage.SendTime,
        LastEditTime: privateMessage.LastEditTime.AsOption(),
        SenderId: privateMessage.SenderId.AsOption(),
        Viewed: privateMessage.Viewed,
        Text: privateMessage.Text,
        Chat: new(
            Id: chatModel.Id,
            CreatorId: chatModel.CreatorId.AsOption(),
            PartecipantId: chatModel.PartecipantId.AsOption(),
            CreationTimestamp: chatModel.CreationTime));

    public async Task<Response<Page<PrivateChatMessageOutput>>> Handle(GetMessagesOfPrivateChat request, CancellationToken cancellationToken)
    {
        var userId = _userInfoProvider.RequireUserId();
        return await _chatContext.PrivateMessages
            .AsNoTracking()
            .Where(m => m.ChatId == request.ChatId)
            .Join(
                _chatContext.PrivateChats.Where(
                    c => c.PartecipantId == userId || c.CreatorId == userId),
                on => on.ChatId,
                on => on.Id,
                (message, chat) => new { Message = message, Chat = chat })
            .OrderByDescending(m => m.Message.SendTime)
            .Select(x =>
                ConvertModelToOutput(x.Message, x.Chat))
            .GetPageAsync(request.Pagination);
    }
}
