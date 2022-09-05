using System.Text;
using System.Threading.Tasks;
using Arcbot.Services;
using Duffet;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.Logging;
using Onebot.Client;
using Quartz;

namespace Arcbot.Units;

public class DebugUnit : UnitBase
{
    private readonly ILogger _logger;
    private readonly ISchedulerFactory _schedulerFactory;


    public DebugUnit(ISchedulerFactory schedulerFactory, ILogger<DebugUnit> logger)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    [Receiver(MessageEventType.Group | MessageEventType.Friend)]
    [Extract("!ping")]
    public string Ping()
    {
        return "pong!";
    }

    [Receiver(MessageEventType.Group | MessageEventType.Friend)]
    [Extract("!version")]
    public StringBuilder Version()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Runtime/{typeof(object).Assembly.GetName().Version}");
        builder.AppendLine($"HyperaiX/{typeof(MessageChain).Assembly.GetName().Version}");
        builder.AppendLine($"Duffet/{typeof(Bank).Assembly.GetName().Version}");
        builder.AppendLine($"Onebot.Net/{typeof(OnebotClient).Assembly.GetName().Version}");
        builder.Append($"Arcbot/{GetType().Assembly.GetName().Version}");
        return builder;
    }


    [Receiver(MessageEventType.Group | MessageEventType.Friend)]
    [Extract("!status")]
    public async Task<StringBuilder> Status()
    {
        var builder = new StringBuilder();
        var apiClient = Context.Client as ApiClient;
        var client = apiClient.client;
        var versionTask = client.GetVersionAsync();
        var statusTask = client.GetStatusAsync();
        var status = await statusTask;
        var version = await versionTask;
        builder.Append(status.Good ? "🟢 " : "🔴 ");
        builder.AppendLine(status.Online ? "Online" : "Offline");
        builder.AppendLine();
        builder.AppendLine($"Impl: {version.Impl}(Onebot{version.OnebotVersion})");
        builder.AppendLine($"Platform: {version.Platform}");
        builder.Append($"Version: {version.Version}");
        var groups = await client.GetGroupListAsync();
        return builder;
    }
}