using ChatService.Application.Queries.PrivateMessages;
using ChatService.Application.Queries.PrivateMessages.Outputs;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Pages;
using EasyDesk.CleanArchitecture.Application.Responses;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.DataAccess.Queries.PrivateMessages;

public class GetMessagesOfPrivateChatHandler : PaginatedQueryHandlerBase<GetMessagesOfPrivateChat, PrivateChatMessageOutput>
{
    protected override Task<Response<Page<PrivateChatMessageOutput>>> Handle(GetMessagesOfPrivateChat request)
    {
        // TODO: implement
        throw new System.NotImplementedException();
    }
}
