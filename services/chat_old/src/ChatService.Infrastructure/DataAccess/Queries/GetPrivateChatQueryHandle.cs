using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatService.Application.Queries;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries;

public class GetPrivateChatQueryHandle : RequestHandlerBase<GetPrivateChat.Query, PrivateChatOutput>
{
    private readonly PrivateChatContext _chatContext;
    private readonly IMapper _mapper;

    public GetPrivateChatQueryHandle(PrivateChatContext chatContext, IMapper mapper)
    {
        _chatContext = chatContext;
        _mapper = mapper;
    }

    protected override async Task<Response<PrivateChatOutput>> Handle(GetPrivateChat.Query request) =>
        await _chatContext
            .PrivateChats
            .Where(u => u.Id == request.ChatId)
            .ProjectTo<PrivateChatOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
}
