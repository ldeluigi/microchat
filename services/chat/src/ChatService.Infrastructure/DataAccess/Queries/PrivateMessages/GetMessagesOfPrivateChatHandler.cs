using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatService.Application.Queries.PrivateMessages;
using ChatService.Application.Queries.PrivateMessages.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateMessages;

public class GetMessagesOfPrivateChatHandler : PaginatedQueryHandlerBase<GetMessagesOfPrivateChat, PrivateChatMessageOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IMapper _mapper;

    public GetMessagesOfPrivateChatHandler(ChatContext chatContext, IMapper mapper)
    {
        _chatContext = chatContext;
        _mapper = mapper;
    }

    protected override async Task<Response<Page<PrivateChatMessageOutput>>> Handle(GetMessagesOfPrivateChat request) =>
        await _chatContext.PrivateMessages
            .Where(m => m.ChatId == request.ChatId)
            .ProjectTo<PrivateChatMessageOutput>(_mapper.ConfigurationProvider)
            .GetPage(request.Pagination);
}
