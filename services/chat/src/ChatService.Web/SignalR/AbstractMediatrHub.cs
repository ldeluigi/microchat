using AutoMapper;
using EasyDesk.CleanArchitecture.Application.Mediator;
using EasyDesk.CleanArchitecture.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Web.SignalR;

public class AbstractMediatrHub : Hub
{
    private IMediator _mediator;
    private IMapper _mapper;

    protected IMediator Mediator => _mediator ??= GetService<IMediator>();

    protected IMapper Mapper => _mapper ??= GetService<IMapper>();

    private T GetService<T>() => Context.GetHttpContext().RequestServices.GetRequiredService<T>();

    protected ActionResultBuilder<TResponse> Command<TResponse>(CommandBase<TResponse> command) =>
        MakeRequest(command);

    protected ActionResultBuilder<TResponse> Query<TResponse>(QueryBase<TResponse> query) =>
        MakeRequest(query);

    private ActionResultBuilder<TResponse> MakeRequest<TResponse>(RequestBase<TResponse> request)
    {
        return null; // new(() => Mediator.Send(request), this);
    }
}
