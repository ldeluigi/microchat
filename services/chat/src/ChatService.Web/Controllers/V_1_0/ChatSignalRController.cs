using ChatService.Web.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0;

public class ChatSignalRController : AbstractMediatrHub
{
    public override Task OnConnectedAsync()
    {
        // (getChatIds for Context.ConnectionId).ForEach( chatId =>
        return Groups.AddToGroupAsync(Context.ConnectionId, "chatId").ContinueWith(_ => base.OnConnectedAsync());
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        // (getChatIds for Context.ConnectionId).ForEach( chatId =>
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, "chatId").ContinueWith(_ => base.OnDisconnectedAsync(exception));
    }

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
        // chatId.ToString()
        return Clients.Group("chatId").SendAsync(text);
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
