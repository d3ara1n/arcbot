// See https://aka.ms/new-console-template for more information

using Arcbot.Clients;
using HyperaiX;
using HyperaiX.Abstractions;
using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var app = Host.CreateApplicationBuilder(args);

app.Services.AddHyperaiX(options =>
{
    options
        .UseLogging()
        .UseBlacklist()
        .UseBots();
});
app.Services.AddSingleton<IEndClient, DummyClient>();

app.Build().Run();