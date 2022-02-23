using HyperaiX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sora.Interfaces;
using Sora.Net;
using Sora.Net.Config;
using Sora.OnebotModel;

namespace Arcbot.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOnebot(this IServiceCollection services, IConfiguration section) => services
            .Configure<OnebotClientOptions>(section)
            .AddSingleton<IApiClient, OnebotClient>();

        public static IServiceCollection AddOnebot(this IServiceCollection services) => services
            .AddSingleton<IApiClient, OnebotClient>();

        public static IServiceCollection AddSora(this IServiceCollection services, IConfiguration section) => services
            .AddHostedService<SoraServer>()
            .AddSingleton<ISoraConfig, ClientConfig>((provider) => {
                var instance = new ClientConfig();
                section.Bind(instance);
                return instance;
            });

    }
}