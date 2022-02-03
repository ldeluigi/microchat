using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Infrastructure.Json;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Web.SignalR;

public class SignalRModule : IAppModule
{
    private readonly bool _detailedErrors;

    public SignalRModule(bool detailedErrors = false)
    {
        _detailedErrors = detailedErrors;
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
            });
    }
}
