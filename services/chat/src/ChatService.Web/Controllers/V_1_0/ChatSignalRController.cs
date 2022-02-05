using ChatService.Web.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0;

public class ChatSignalRController : AbstractMediatrHub
{
    [HubMethodName("chat.createWith")]
    public async Task CreateChatWith(Guid userId)
    {

    }

    [HubMethodName("chat.delete")]
    public async Task DeleteChat(Guid chatId)
    {

    }

    [HubMethodName("message.send")]
    public async Task SendMessage(Guid chatId, string text)
    {

    }

    [HubMethodName("message.edit")]
    public async Task EditMessage(Guid messageId, string text)
    {

    }

    [HubMethodName("message.delete")]
    public async Task DeleteMessage(Guid messageId)
    {

    }
}
