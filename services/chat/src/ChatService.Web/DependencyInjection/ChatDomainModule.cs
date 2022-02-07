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
using System;

namespace ChatService.Web.DependencyInjection;

public class ChatDomainModule : IAppModule
{
    public class TestScope
    {
        public Guid Random { get; set; } = Guid.NewGuid();
    }

    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPrivateChatRepository, PrivateChatRepository>()
            .AddScoped<IPrivateMessageRepository, PrivateMessageRepository>()
            .AddScoped<UserLifecycleService>()
            .AddScoped<PrivateChatLifecycleService>()
            .AddScoped<TestScope>();
        services
            .AddSingleton<IModelConverter<User, UserModel>, UserModelConverter>()
            .AddSingleton<IModelConverter<PrivateChat, PrivateChatModel>, PrivateChatModelConverter>()
            .AddSingleton<IModelConverter<PrivateMessage, PrivateMessageModel>, PrivateMessageModelConverter>();
    }
}
