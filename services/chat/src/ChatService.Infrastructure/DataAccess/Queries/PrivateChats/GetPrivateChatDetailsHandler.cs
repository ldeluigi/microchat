using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatDetailsHandler : RequestHandlerBase<GetPrivateChatDetails, DetailedPrivateChatOutput>
{
    private readonly ChatContext _chatContext;

    public GetPrivateChatDetailsHandler(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    protected override Task<Response<DetailedPrivateChatOutput>> Handle(GetPrivateChatDetails request)
    {
        // TODO: implement
        throw new System.NotImplementedException();
    }
}
