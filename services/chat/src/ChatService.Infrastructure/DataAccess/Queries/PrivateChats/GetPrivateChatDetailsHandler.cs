using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Dal.EfCore.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatDetailsHandler : RequestHandlerBase<GetPrivateChatDetails, DetailedPrivateChatOutput>
{
    private readonly ChatContext _chatContext;
    private readonly IMapper _mapper;

    public GetPrivateChatDetailsHandler(ChatContext chatContext, IMapper mapper)
    {
        _chatContext = chatContext;
        _mapper = mapper;
    }

    protected override Task<Response<DetailedPrivateChatOutput>> Handle(GetPrivateChatDetails request) =>
        _chatContext.PrivateChats
            .Where(c => c.Id == request.Id)
            .ProjectTo<DetailedPrivateChatOutput>(_mapper.ConfigurationProvider)
            .FirstOptionAsync()
            .ThenOrElseError(Errors.NotFound);
}
