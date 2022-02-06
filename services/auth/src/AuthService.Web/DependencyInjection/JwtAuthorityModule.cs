using System;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Passwords;
using AuthService.Infrastructure.Authentication;
using AuthService.Infrastructure.PasswordHashing;
using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.CleanArchitecture.Infrastructure.Configuration;
using EasyDesk.CleanArchitecture.Infrastructure.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthService.Web.DependencyInjection;

public class JwtAuthorityModule : IAppModule
{
    private readonly IConfiguration _configuration;

    public JwtAuthorityModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services
            .AddSingleton(_configuration.RequireValue<TokensSettings>("TokensSettings"))
            .AddSingleton<JwtTokenConfiguration>()
            .AddScoped<IHashingService, HashingService>()
            .AddScoped<ISaltGenerator, RandomSaltGenerator>()
            .AddScoped<IHashCalculator, Pbkdf2Hashing>()
            .AddScoped(s => CreateAccessTokenService(s, app));
    }

    private IAccessTokenService CreateAccessTokenService(IServiceProvider provider, AppDescription appDescription)
    {
        var timestampProvider = provider.GetRequiredService<ITimestampProvider>();
        return new JwtAccessTokenService(
            new JwtFacade(timestampProvider),
            JwtConfigurationUtils.GetJwtTokenConfiguration(_configuration, appDescription.Name, timestampProvider),
            JwtConfigurationUtils.GetJwtValidationConfiguration(_configuration, appDescription.Name),
            provider.GetRequiredService<ILogger<JwtAccessTokenService>>());
    }
}
