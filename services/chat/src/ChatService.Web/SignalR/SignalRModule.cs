using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Infrastructure.Json;
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
                configure.Configuration.ClientName = "MicrochatSignalR";
            });
    }
}
