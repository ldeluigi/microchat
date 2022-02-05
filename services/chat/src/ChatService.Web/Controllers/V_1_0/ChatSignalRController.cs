using ChatService.Web.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0;

public class ChatSignalRController : AbstractMediatrHub
{
    [HubMethodName("create-chat")]
    public async Task CreateChat(Guid withId)
    {

    }
}
