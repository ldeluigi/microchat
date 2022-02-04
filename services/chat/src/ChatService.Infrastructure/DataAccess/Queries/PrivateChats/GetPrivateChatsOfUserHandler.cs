using ChatService.Application.Queries.PrivateChats;
using ChatService.Application.Queries.PrivateChats.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateChats;

public class GetPrivateChatsOfUserHandler : PaginatedQueryHandlerBase<GetPrivateChatsOfUser, PrivateChatOfUserOutput>
{
    private readonly ChatContext _chatContext;

    public GetPrivateChatsOfUserHandler(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }

    protected override Task<Response<Page<PrivateChatOfUserOutput>>> Handle(GetPrivateChatsOfUser request)
    {
        // TODO: implement
        throw new System.NotImplementedException();
    }
}
