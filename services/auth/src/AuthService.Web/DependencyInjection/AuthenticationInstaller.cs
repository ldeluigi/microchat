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
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddSingleton(configuration.RequireValue<TokensSettings>("TokensSettings"));
            services
                .AddScoped<IHashingService, HashingService>()
                .AddScoped<ISaltGenerator, RandomSaltGenerator>()
                .AddScoped<IHashCalculator, Pbkdf2Hashing>()
                .AddScoped(CreateAccessTokenService);

            ////services.AddAuthorization(options =>
            ////{
            ////    options.DefaultPolicy = new AuthorizationPolicyBuilder()
            ////        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            ////        .AddAuthenticationSchemes(ApiKeyDefaults.Scheme)
            ////        .RequireAuthenticatedUser()
            ////        .Build();
            ////});
        }

        private IAccessTokenService CreateAccessTokenService(IServiceProvider provider)
        {
            return new JwtAccessTokenService(provider.GetRequiredService<JwtService>(), provider.GetRequiredService<JwtSettings>());
        }
    }
}
