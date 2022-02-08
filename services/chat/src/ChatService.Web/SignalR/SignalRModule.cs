using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Infrastructure.Json;
using EasyDesk.Tools.Options;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Web.SignalR;

public class SignalRModule : IAppModule
{
    private readonly bool _detailedErrors;
    private readonly string _redisConnectionString;

    public SignalRModule(string redisConnectionString, bool detailedErrors = false)
    {
        _detailedErrors = detailedErrors;
        _redisConnectionString = redisConnectionString;
    }

    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services
            .AddSignalR(configure =>
            {
                configure.EnableDetailedErrors = _detailedErrors;
            })
            .AddNewtonsoftJsonProtocol(configure =>
            {
                configure.PayloadSerializerSettings = JsonDefaults.DefaultSerializerSettings();
            })
            .AddStackExchangeRedis(_redisConnectionString, configure =>
            {
                configure.Configuration.ClientName = app.Name;
            });

        services.AddSingleton<IUserIdProvider, DefaultUserIdProvider>();
    }

    private class DefaultUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection) =>
            connection
            .GetHttpContext()
            .RequestServices
            .GetRequiredService<IUserInfoProvider>()
            .GetUserInfo()
            .Map(u => u.UserId)
            .OrElseNull();
    }
}
