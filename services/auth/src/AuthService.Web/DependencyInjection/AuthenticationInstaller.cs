using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Passwords;
using AuthService.Infrastructure.Authentication;
using AuthService.Infrastructure.PasswordHashing;
using EasyDesk.CleanArchitecture.Infrastructure.Configuration;
using EasyDesk.CleanArchitecture.Infrastructure.Jwt;
using EasyDesk.CleanArchitecture.Web.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AuthService.Web.DependencyInjection
{
    public class AuthenticationInstaller : IServiceInstaller
    {
        public static readonly string JwtScopeName = "Global";

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services
                .AddSingleton(configuration.RequireValue<TokensSettings>("TokensSettings"))
                .AddSingleton(configuration.ReadJwtSettings(JwtScopeName))
                .AddScoped<IHashingService, HashingService>()
                .AddScoped<ISaltGenerator, RandomSaltGenerator>()
                .AddScoped<IHashCalculator, Pbkdf2Hashing>()
                .AddScoped(CreateAccessTokenService);
        }

        private IAccessTokenService CreateAccessTokenService(IServiceProvider provider)
        {
            return new JwtAccessTokenService(provider.GetRequiredService<JwtService>(), provider.GetRequiredService<JwtSettings>());
        }
    }
}
