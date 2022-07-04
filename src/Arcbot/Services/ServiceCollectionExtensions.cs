using HyperaiX;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiClient(this IServiceCollection services)
    {
        return services
            .AddSingleton<IApiClient, ApiClient>();
    }
}