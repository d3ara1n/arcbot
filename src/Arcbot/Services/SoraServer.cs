using System;
using System.Threading;
using System.Threading.Tasks;
using HyperaiX;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sora;
using Sora.Interfaces;
namespace Arcbot.Services;

public class SoraServer : IHostedService
{
    private readonly ILogger _logger;
    private readonly ISoraConfig _config;
    private readonly OnebotClient _client;

    private ISoraService service;
    public SoraServer(ILogger<SoraServer> logger, ISoraConfig config, IApiClient client)
    {
        _logger = logger;
        _config = config;
        _client = (OnebotClient)client;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        service = SoraServiceFactory.CreateService(_config, except => _logger.LogCritical(except, "Sora error logged"));
        service.Event.OnGroupMessage += async (message_type, args) => await _client.ForwardAsync(args);
        await service.StartService();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        ((IDisposable)service)?.Dispose();
        return Task.CompletedTask;
    }
}