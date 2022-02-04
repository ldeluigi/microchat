using ChatService.Application.Queries;
using ChatService.Web.SignalR;
using EasyDesk.CleanArchitecture.Web.Dto;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatService.Web.Controllers.V_1_0;

public class ChatSignalRController : AbstractMediatrHub
{
    public async Task HelloWorld()
    {
        var res = await Mediator.Send(new TestQuery());
        await Clients.Caller.SendAsync(
            "hello-world",
            res.IsSuccess ? ResponseDto.FromData(res.Value) : ResponseDto.FromError(ErrorDto.FromError(res.Error.Value)));
    }
}
