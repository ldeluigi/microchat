using EasyDesk.CleanArchitecture.Application.Authorization;
using EasyDesk.CleanArchitecture.Application.Modules;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace UserService.Web.DependencyInjection;

public class PermissionsModule : IAppModule
{
    public class PermissionProvider : IPermissionsProvider
    {
        public Task<IImmutableSet<Permission>> GetPermissionsForUser(UserInfo userDescription)
        {
            return Task.FromResult<IImmutableSet<Permission>>(ImmutableHashSet.Create<Permission>());
        }
    }

    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services.AddSingleton<IPermissionsProvider, PermissionProvider>();
    }
}
