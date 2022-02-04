﻿using ChatService.Application.Messages.AccountLifecycle;
using EasyDesk.CleanArchitecture.Application.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Bus;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Web.DependencyInjection;

public class TopicSubscriberModule : IAppModule
{
    private class TopicSubscriberService : IHostedService
    {
        private readonly IBus _rebus;

        public TopicSubscriberService(IBus rebus)
        {
            _rebus = rebus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _rebus.Subscribe<AccountCreated>();
            await _rebus.Subscribe<AccountDeleted>();
        }

        public Task StopAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }

    public void ConfigureServices(IServiceCollection services, AppDescription app)
    {
        services.AddHostedService<TopicSubscriberService>();
    }
}
