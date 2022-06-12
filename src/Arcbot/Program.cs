using Arcbot;
using Arcbot.Logging.Formatters;
using Arcbot.Services;
using Ac682.Extensions.Logging.Console;
using HyperaiX;
using HyperaiX.Units;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configure => configure
        .AddTomlFile("arc.is.bot.toml"))
    .ConfigureServices((context, services) => services
        .AddLogging(builder => builder
            .ClearProviders()
            .AddConsole(configure => configure
                .SetMinimalLevel(LogLevel.Debug)
                .AddFormatter<MessageElementFormatter>()
                .AddBuiltinFormatters()))
        .AddApiClient().Configure<ApiClientOptions>(context.Configuration.GetSection("Onebot"))
        .AddHyperaiX(configure => configure
            .UseEventBlocker()
            .UseLogging()
            .UseUnits())
        .AddUnits(configure => configure.MapUnits())
        .AddSingleton<ClasstableService>()
        .AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseInMemoryStore();
        }).Configure<QuartzOptions>(context.Configuration.GetSection("Quartz"))
        .AddQuartzHostedService()
        .Configure<QuartzHostedServiceOptions>(context.Configuration.GetSection("QuartzService"))
        .AddSingleton<SelfStore>())
    .UseConsoleLifetime()
    .Build()
    .Run();