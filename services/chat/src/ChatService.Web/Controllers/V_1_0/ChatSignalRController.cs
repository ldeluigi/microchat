using ChatService.Web.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0;

public class ChatSignalRController : AbstractMediatrHub
{
    public async Task HelloWorld()
    {
        await Clients.Caller.SendAsync(
            "hello-world",
            "kek");
    }
}
