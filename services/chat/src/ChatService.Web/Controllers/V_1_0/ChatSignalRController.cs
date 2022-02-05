using ChatService.Web.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0;

public class ChatSignalRController : AbstractMediatrHub
{
    [HubMethodName("chat.createWith")]
    public Task CreateChatWith(Guid userId)
    {
        throw new NotImplementedException();
    }

    [HubMethodName("chat.delete")]
    public Task DeleteChat(Guid chatId)
    {
        throw new NotImplementedException();
    }

    [HubMethodName("message.send")]
    public Task SendMessage(Guid chatId, string text)
    {
        throw new NotImplementedException();
    }

    [HubMethodName("message.edit")]
    public Task EditMessage(Guid messageId, string text)
    {
        throw new NotImplementedException();
    }

    [HubMethodName("message.delete")]
    public Task DeleteMessage(Guid messageId)
    {
        throw new NotImplementedException();
    }
}
