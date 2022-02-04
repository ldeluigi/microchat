using EasyDesk.CleanArchitecture.Application.Modules;

namespace ChatService.Web.DependencyInjection;

public static class AppBuilderExtensions
{
    public static AppBuilder AddModule<T>(this AppBuilder builder)
        where T : IAppModule, new() =>
        builder.AddModule(new T());
}
