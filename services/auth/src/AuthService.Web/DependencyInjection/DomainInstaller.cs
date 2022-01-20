using AuthService.Domain.Aggregates.AccountAggregate;
using AuthService.Domain.Authentication;
using AuthService.Domain.Authentication.Accounts;
using AuthService.Domain.Authentication.Passwords;
using AuthService.Domain.Emails;
using AuthService.Infrastructure.Authentication;
using AuthService.Infrastructure.DataAccess.Model.AccountAggregate;
using AuthService.Infrastructure.DataAccess.ModelConverters;
using AuthService.Infrastructure.DataAccess.Repositories;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using EasyDesk.CleanArchitecture.Domain.Time;
using EasyDesk.CleanArchitecture.Web.DependencyInjection;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AuthService.Web.DependencyInjection
{
    /// <summary>
    /// An installer containing dependency injection configuration for the domain layer.
    /// </summary>
    public class DomainInstaller : IServiceInstaller
    {
        /// <inheritdoc/>
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddSingleton<IModelConverter<Account, AccountModel>, AccountConverter>();

            services
                .AddScoped(PasswordService)
                .AddScoped(AuthenticationService)
                .AddScoped(EmailService)
                .AddScoped<AccountRegistrationMethod>()
                .AddScoped<AccountAuthenticationMethod>()
                .AddScoped<AccountLifecycleService>()
                .AddScoped<LoginService>()
                .AddScoped<RefreshService>();
        }

        private EmailManagementService EmailService(IServiceProvider provider) =>
            new(
                provider.GetRequiredService<ITimestampProvider>(),
                Duration.FromTimeSpan(provider.GetRequiredService<TokensSettings>().AccountVerificationTokenLifetime));

        private IAuthenticationService AuthenticationService(IServiceProvider provider) =>
            new AuthenticationService(
                provider.GetRequiredService<IAccessTokenService>(),
                provider.GetRequiredService<ITimestampProvider>(),
                Duration.FromTimeSpan(provider.GetRequiredService<TokensSettings>().RefreshTokenLifetime));

        private PasswordService PasswordService(IServiceProvider provider) =>
            new(
                provider.GetRequiredService<IHashingService>(),
                provider.GetRequiredService<ITimestampProvider>(),
                Duration.FromTimeSpan(provider.GetRequiredService<TokensSettings>().AccountVerificationTokenLifetime));
    }
}
