// See https://aka.ms/new-console-template for more information

using Arcbot.Clients;
using Arcbot.Modules.Debugging;
using HyperaiX;
using HyperaiX.Abstractions;
using HyperaiX.Clients.Lagrange;
using HyperaiX.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var app = Host.CreateApplicationBuilder(args);

app.Logging.ClearProviders().AddSimpleConsole();
app.Services.AddMemoryCache();

#region HyperaiX Services

app.AddHyperaiX(configuration =>
{
    configuration
        .UseLogging()
        .UseBlacklist()
        .UseBots();

    configuration
        .Mount<DebugModule>();
});
app.Services.AddLagrangeClient();

#endregion

app.Build().Run();