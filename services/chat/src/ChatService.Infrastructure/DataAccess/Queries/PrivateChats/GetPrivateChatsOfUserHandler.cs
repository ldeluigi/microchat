using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatsOfUserHandler : PaginatedQueryHandlerBase<GetPrivateChatsOfUser, PrivateChatOfUserOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IMapper _mapper;

    public GetPrivateChatsOfUserHandler(ChatContext chatContext, IMapper mapper)
    {
        _chatContext = chatContext;
        _mapper = mapper;
    }

    protected override async Task<Response<Page<PrivateChatOfUserOutput>>> Handle(GetPrivateChatsOfUser request) =>
        await _chatContext.PrivateChats
            .Where(c => c.CreatorId.Equals(request.UserId) || c.PartecipantId.Equals(request.UserId))
            .ProjectTo<PrivateChatOfUserOutput>(_mapper.ConfigurationProvider)
            .GetPage(request.Pagination);
}
