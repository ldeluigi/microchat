using AutoMapper;
using EasyDesk.CleanArchitecture.Application.ErrorManagement;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Application.Responses;
using EasyDesk.CleanArchitecture.Web.Dto;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ChatService.Web.SignalR;

//[Authorize]
public class AbstractMediatrHub : Hub
{
    private IMapper _mapper;
    private IServiceScopeFactory _scopeFactory;

    protected IMapper Mapper => _mapper ??= GetService<IMapper>();

    private T GetService<T>() => Context.GetHttpContext().RequestServices.GetRequiredService<T>();

    protected Task<Response<TResponse>> Command<TResponse>(CommandBase<TResponse> command) =>
        MakeRequest(command);

    protected Task<Response<TResponse>> Query<TResponse>(QueryBase<TResponse> query) =>
        MakeRequest(query);

    protected IServiceScopeFactory ScopeFactory => _scopeFactory ??= GetService<IServiceScopeFactory>();

    protected IClientProxy Caller => Clients.Caller;

    private async Task<Response<TResponse>> MakeRequest<TResponse>(RequestBase<TResponse> request)
    {
        using (var scope = ScopeFactory.CreateScope())
        {
            return await scope.ServiceProvider.GetRequiredService<IMediator>().Send(request);
        }
    }

    protected Task SendError(Error error) => Caller.SendAsync("error", ErrorDto.FromError(error));
}
