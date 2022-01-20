using EasyDesk.CleanArchitecture.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AuthService.Web;

/// <summary>
/// Main class of the application.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return EntryPoint.CreateHostBuilder<Startup>(args, "MICROCHAT_");
    }
}
