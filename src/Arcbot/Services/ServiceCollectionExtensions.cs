using HyperaiX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Services
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddApiClient(this IServiceCollection services) => services
            .AddSingleton<IApiClient, ApiClient>();
    }
}