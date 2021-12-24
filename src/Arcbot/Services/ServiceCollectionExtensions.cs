using HyperaiX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onebot.Protocol.Communications;

namespace Arcbot.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOnebot(this IServiceCollection services, IConfiguration section) => services
            .Configure<OnebotClientOptions>(section)
            .AddSingleton<IApiClient, OnebotClient>();

        public static IServiceCollection AddOnebot(this IServiceCollection services) => services
            .AddSingleton<IApiClient, OnebotClient>();
    }
}