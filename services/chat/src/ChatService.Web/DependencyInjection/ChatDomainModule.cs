using ChatService.Domain;
using ChatService.Domain.Aggregates.MessageAggregate;
using ChatService.Domain.Aggregates.PrivateChatAggregate;
using ChatService.Domain.Aggregates.UserAggregate;
using ChatService.Infrastructure.DataAccess.Model.ChatAggregate;
using ChatService.Infrastructure.DataAccess.Model.MessageAggregate;
using ChatService.Infrastructure.DataAccess.Model.UserAggregate;
using ChatService.Infrastructure.DataAccess.ModelConverters;
using ChatService.Infrastructure.DataAccess.Repositories;
using EasyDesk.CleanArchitecture.Application.Modules;
using EasyDesk.CleanArchitecture.Dal.EfCore.ModelConversion;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Web.DependencyInjection;

public class ChatDomainModule : IAppModule
{
    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPrivateChatRepository, PrivateChatRepository>()
            .AddScoped<IPrivateMessageRepository, PrivateMessageRepository>()
            .AddScoped<UserLifecycleService>()
            .AddScoped<PrivateChatLifecycleService>();
        services
            .AddSingleton<IModelConverter<User, UserModel>, UserModelConverter>()
            .AddSingleton<IModelConverter<PrivateChat, PrivateChatModel>, PrivateChatModelConverter>()
            .AddSingleton<IModelConverter<PrivateMessage, PrivateMessageModel>, PrivateMessageModelConverter>();
    }
}
