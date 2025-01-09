// See https://aka.ms/new-console-template for more information

using Arcbot.Clients;
using Arcbot.Modules.Debugging;
using HyperaiX;
using HyperaiX.Abstractions;
using HyperaiX.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



var app = Host.CreateApplicationBuilder(args);

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
app.Services.AddSingleton<IEndClient, DummyClient>();

#endregion

app.Build().Run();