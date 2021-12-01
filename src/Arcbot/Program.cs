using System;
using System.Threading.Tasks;
using Ac682.Extensions.Logging.Console;
using Arcbot.Logging.Formatters;
using Arcbot.Services;
using HyperaiX;
using HyperaiX.Clients;
using HyperaiX.Units;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Arcbot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configure => configure
                    .AddTomlFile("arc.is.bot.toml"))
                .ConfigureServices((context, services) => services
                    .AddLogging(builder => builder
                        .ClearProviders()
                        .AddConsole(configure => configure
                            .AddFormatter<MessageElementFormatter>()
                            .AddBuiltinFormatters()))
                    .AddOnebot(context.Configuration.GetSection("Onebot"))
                    .AddHyperaiX(configure => configure
                        .UseEventBlocker()
                        .UseLogging()
                        .UseUnits())
                    .AddUnits(configure => configure.MapUnits()))
                .UseConsoleLifetime()
                .Build()
                .Run();
        }
    }
}