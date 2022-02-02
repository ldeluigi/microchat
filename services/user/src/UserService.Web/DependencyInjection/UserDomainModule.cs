using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Infrastructure.DataAccess.Model.UserAggregate;
using UserService.Infrastructure.DataAccess.ModelConverters;
using UserService.Infrastructure.DataAccess.Repositories;

namespace UserService.Web.DependencyInjection;

public class UserDomainModule : IAppModule
{
    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddSingleton<IModelConverter<User, UserModel>, UserConverter>();
    }
}
