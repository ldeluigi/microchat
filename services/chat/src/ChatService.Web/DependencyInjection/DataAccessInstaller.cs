using EasyDesk.CleanArchitecture.Dal.EfCore;
using EasyDesk.CleanArchitecture.Web.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Web.DependencyInjection
{
    /// <summary>
    /// An installer containing dependency injection configuration for the data access layer.
    /// </summary>
    public class DataAccessInstaller : IServiceInstaller
    {
        /// <inheritdoc/>
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddEfCoreDataAccess(configuration.GetConnectionString("MainDb"), options =>
            {
            });
        }
    }
}
