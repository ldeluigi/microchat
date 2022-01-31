using EasyDesk.CleanArchitecture.Web.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Aggregates.UserService;
using UserService.Infrastructure.DataAccess.Repositories;

namespace UserService.Web.DependencyInjection;

public class DomainInstaller : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        // services.AddSingleton<IModelConverter<User, UserModel>, UserConverter>();
        services.AddScoped<UserRegistrationMethod>();
    }
}
