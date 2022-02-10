using ChatService.Application.Commands.PrivateChats;
using ChatService.Application.Commands.PrivateMessages;
using ChatService.Web.Controllers.V_1_0.SignalRChatDtos;
using ChatService.Web.SignalR;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.Tools.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0;

[Authorize]
public class ChatSignalRController : AbstractMediatrHub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("chat.createWith")]
    public async Task CreateChatWith(Guid userId)
    {
        var command = new CreatePrivateChat.Command(userId);
        var res = await Command(command);
        await res.IfFailureAsync(SendError);
        await res.IfSuccessAsync(async chat =>
        {
            await Clients.Users(chat.Members.Select(g => g.ToString())).SendAsync("chat.created", Mapper.Map<PrivateChatDto>(chat));
        });
    }

    [HubMethodName("chat.delete")]
    public async Task DeleteChat(Guid chatId)
    {
        var command = new DeletePrivateChat.Command(chatId);
        var res = await Command(command);
        await res.IfFailureAsync(SendError);
        await res.IfSuccessAsync(async chat =>
        {
            await Clients.Users(chat.Members.Select(g => g.ToString())).SendAsync("chat.deleted", Mapper.Map<PrivateChatDto>(chat));
        });
    }

    [HubMethodName("message.send")]
    public async Task SendMessage(Guid chatId, string text)
    {
        var command = new SendPrivateMessage.Command(chatId, text);
        var res = await Command(command);
        await res.IfFailureAsync(SendError);
        await res.IfSuccessAsync(async message =>
        {
            await Clients.Users(message.Chat.MembersIds()).SendAsync("message.received", Mapper.Map<PrivateMessageDto>(message));
        });
    }

    [HubMethodName("message.edit")]
    public async Task EditMessage(Guid messageId, string text)
    {
        var command = new EditPrivateMessage.Command(messageId, text);
        var res = await Command(command);
        await res.IfFailureAsync(SendError);
        await res.IfSuccessAsync(async message =>
        {
            await Clients.Users(message.Chat.MembersIds()).SendAsync("message.edited", Mapper.Map<PrivateMessageDto>(message));
        });
    }

    [HubMethodName("message.view")]
    public async Task ViewMessage(Guid messageId)
    {
        var command = new ViewPrivateMessage.Command(messageId);
        var res = await Command(command);
        await res.IfFailureAsync(SendError);
        await res.IfSuccessAsync(async message =>
        {
            await Clients.Users(message.Chat.MembersIds()).SendAsync("message.viewed", Mapper.Map<PrivateMessageDto>(message));
        });
    }

    [HubMethodName("message.delete")]
    public async Task DeleteMessage(Guid messageId)
    {
        var command = new DeletePrivateMessage.Command(messageId);
        var res = await Command(command);
        await res.IfFailureAsync(SendError);
        await res.IfSuccessAsync(async message =>
        {
            await Clients.Users(message.Chat.MembersIds()).SendAsync("message.deleted", Mapper.Map<PrivateMessageDto>(message));
        });
    }
}
