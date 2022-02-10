using System;
using EasyDesk.CleanArchitecture.Application.Authorization.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Data.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Messaging.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Dal.EfCore.DependencyInjection;
using EasyDesk.CleanArchitecture.Infrastructure.Configuration;
using EasyDesk.CleanArchitecture.Infrastructure.Jwt;
using EasyDesk.CleanArchitecture.Web.Authentication.Jwt;
using EasyDesk.CleanArchitecture.Web.Startup;
using EasyDesk.CleanArchitecture.Web.Startup.Modules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.DataAccess;
using UserService.Web.DependencyInjection;

namespace UserService.Web;

/// <summary>
/// The boostrapper of the application.
/// </summary>
public class Startup : BaseStartup
{
    private const string SectionName = "JwtScopes:Global";

    /// <summary>
    /// Creates a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">The configuration of the application.</param>
    /// <param name="environment">The environment in which the application runs.</param>
    public Startup(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
    {
    }

    protected override Type ApplicationAssemblyMarker => typeof(ApplicationMarker);

    protected override Type InfrastructureAssemblyMarker => typeof(InfrastructureMarker);

    protected override Type WebAssemblyMarker => typeof(Startup);

    protected override string ServiceName => "UserService";

    public override void ConfigureApp(AppBuilder builder)
    {
        var shouldApplyMigrations = Configuration
                    .RequireSection("Migrations")
                    .RequireValue<bool>("RunOnStartup");
        builder
            .AddApiVersioning()
            .AddDataAccess(new EfCoreDataAccess<UserContext>(
                Configuration.GetConnectionString("MainDb"),
                applyMigrations: Environment.IsDevelopment() || shouldApplyMigrations))
            .AddSwagger()
            .AddAuthentication(options =>
                options.AddJwtBearer(nameof(JwtBearerScheme), options =>
                    options.ConfigureValidationParameters(Configuration.GetJwtValidationConfiguration(SectionName))))
            .AddAuthorization()
            .AddModule<UserDomainModule>()
            .AddRebusMessaging(configure =>
                configure
                    .UseOutbox()
                    .ConfigureTransport(t =>
                        t.UseRabbitMq(Configuration.GetConnectionString("RabbitMq"), ServiceName)))
            .AddModule<TopicSubscriberModule>();
    }
}
