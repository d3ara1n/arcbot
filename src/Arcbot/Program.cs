using Ac682.Extensions.Logging.Console;
using Arcbot;
using Arcbot.Data;
using Arcbot.Logging.Formatters;
using HyperaiX;
using HyperaiX.Units;
using Microsoft.EntityFrameworkCore;
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
                .AddBuiltinFormatters())
            .AddFile("logs/arcbot-{Date}.log")
            .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning))
        .AddApiClient().Configure<ApiClientOptions>(context.Configuration.GetSection("Onebot"))
        .AddHyperaiX(configure => configure
            .UseEventBlocker()
            .UseLogging()
            .UseUnits())
        .AddUnits(configure => configure.LookForUnits())
        .AddModules(context.Configuration)
        .AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(4);
        }).Configure<QuartzOptions>(context.Configuration.GetSection("Quartz"))
        .AddQuartzHostedService()
        .Configure<QuartzHostedServiceOptions>(context.Configuration.GetSection("QuartzService"))
        .AddMemoryCache()
        .AddDbContext<ArcContext>(options => options.UseSqlite(context.Configuration.GetConnectionString("Arcbot"))
            .LogTo(
                s => { })))
    .UseConsoleLifetime()
    .Build()
    .Run();