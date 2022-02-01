using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Infrastructure.DataAccess;
using AuthService.Web.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Authorization.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Authorization.RoleBased.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Data.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Messaging.DependencyInjection;
using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Dal.EfCore.DependencyInjection;
using EasyDesk.CleanArchitecture.Messaging.ServiceBus.DependencyInjection;
using EasyDesk.CleanArchitecture.Web.Authentication.Jwt;
using EasyDesk.CleanArchitecture.Web.Startup;
using EasyDesk.CleanArchitecture.Web.Startup.Modules;
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

    public override void ConfigureApp(AppBuilder builder)
    {
        builder
            .AddApiVersioning()
            .AddDataAccess(new EfCoreDataAccess<AuthContext>(Configuration, applyMigrations: Environment.IsDevelopment()))
            .AddSwagger()
            .AddAuthentication(options =>
                options.AddScheme(new JwtBearerScheme(options =>
                    options
                    .UseJwtSettingsFromConfiguration(
                        Configuration,
                        JwtAuthorityModule.JwtScopeName))))
            .AddAuthorization(configure =>
                configure.UseRoleBasedPermissions())
            .AddModule(new JwtAuthorityModule(Configuration))
            .AddModule(new AuthDomainModule())
            .AddMessaging(new AzureServiceBus(Configuration, prefix: Environment.EnvironmentName), options =>
                options.AddOutboxSender());
    }
}
