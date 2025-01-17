// See https://aka.ms/new-console-template for more information

using Arcbot.Clients;
using Arcbot.Console;
using Arcbot.Console.Formatters;
using Arcbot.Modules.Debugging;
using HyperaiX;
using HyperaiX.Abstractions;
using HyperaiX.Clients.Lagrange;
using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var app = Host.CreateApplicationBuilder(args);

app.Logging.ClearProviders().AddConsole(options => options.FormatterName = "FancyConsoleFormatter")
    .AddConsoleFormatter<FancyConsoleFormatter, FancyConsoleFormatterOptions>(options =>
    {
        options.TimestampFormat = "t";
        options.IncludeScopes = true;

        foreach (var type in typeof(IConsoleFormatter).Assembly.GetExportedTypes().Where(x =>
                     x is { IsPublic: true, IsClass: true, IsAbstract: false } &&
                     x.IsAssignableTo(typeof(IConsoleFormatter))))
        {
            options.Formatters.Add(type);
        }
    });
app.Services.AddMemoryCache();

#region HyperaiX Services

app.AddHyperaiX(configuration =>
{
    configuration
        .UseErrorLogging()
        .UseBlacklist()
        .UseBots()
        .UseUnits(options => options.AddElementType(typeof(Program).Assembly));

    configuration
        .Mount<DebugModule>();
});
app.Services.AddLagrangeClient();
// app.Services.AddSingleton<IEndClient, DummyClient>();

#endregion

app.Build().Run();