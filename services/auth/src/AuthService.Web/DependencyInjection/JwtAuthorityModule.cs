using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Passwords;
using AuthService.Infrastructure.Authentication;
using AuthService.Infrastructure.PasswordHashing;
using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Infrastructure.Configuration;
using EasyDesk.CleanArchitecture.Infrastructure.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Web.DependencyInjection;

public class JwtAuthorityModule : IAppModule
{
    public const string JwtScope = "JwtScopes:Global";
    private readonly IConfiguration _configuration;

    public JwtAuthorityModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services
            .AddSingleton(_configuration.RequireValue<TokensSettings>("TokensSettings"))
            .AddSingleton(_configuration.GetJwtTokenConfiguration(JwtScope))
            .AddSingleton(_configuration.GetJwtValidationConfiguration(JwtScope))
            .AddScoped<IHashingService, HashingService>()
            .AddScoped<ISaltGenerator, RandomSaltGenerator>()
            .AddScoped<IHashCalculator, Pbkdf2Hashing>()
            .AddScoped<IAccessTokenService, JwtAccessTokenService>();
    }
}
