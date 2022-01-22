using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Infrastructure.DataAccess;
using AuthService.Web.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Data.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Events.DependencyInjection;
using EasyDesk.CleanArchitecture.Dal.EfCore.DependencyInjection;
using EasyDesk.CleanArchitecture.Infrastructure.Events.ServiceBus;
using EasyDesk.CleanArchitecture.Web.Authentication.Jwt;
using EasyDesk.CleanArchitecture.Web.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace AuthService.Web;

/// <summary>
/// The boostrapper of the application.
/// </summary>
public class Startup : BaseStartup
{
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

    protected override string ServiceName => "Auth";

    protected override bool UsesPublisher => true;

    protected override bool UsesConsumer => false;

    protected override bool IsMultitenant => false;

    protected override bool UsesSwagger => true;

    protected override void SetupAuthentication(AuthenticationOptions options) =>
        options.AddScheme(new JwtBearerScheme(options =>
        options
        .UseJwtSettingsFromConfiguration(
            Configuration,
            AuthenticationInstaller.JwtScopeName)));

    protected override IDataAccessImplementation DataAccessImplementation =>
        new EfCoreDataAccess<AuthContext>(Configuration, applyMigrations: Environment.IsDevelopment());

    protected override IEventBusImplementation EventBusImplementation =>
        new AzureServiceBus(Configuration, prefix: Environment.EnvironmentName);
}
