using System;
using System.Linq;
using System.Runtime.Loader;
using HyperaiX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiClient(this IServiceCollection services)
    {
        return services
            .AddSingleton<IApiClient, ApiClient>();
    }

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (var type in AssemblyLoadContext.All.SelectMany(x => x.Assemblies.SelectMany(y => y.GetExportedTypes()))
                     .Where(z => z != typeof(ModuleBase) && z.IsAssignableTo(typeof(ModuleBase))))
        {
            var module = Activator.CreateInstance(type) as ModuleBase;
            module!.ConfigureServices(services, configuration);
        }

        return services;
    }
}