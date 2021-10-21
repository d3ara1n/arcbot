using System.Diagnostics.Tracing;
using HyperaiX.Abstractions.Events;
using HyperaiX.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Arcbot
{
    public static class HyperaiConfigurationBuilderExtensions
    {

        public static HyperaiXConfigurationBuilder UseEventBlocker(this HyperaiXConfigurationBuilder builder) =>
        builder.Use((evt, pvd, nxt) =>
            {
                if (evt is not UnknownEventArgs) nxt(evt, pvd);
            });

        public static HyperaiXConfigurationBuilder UseLogging(this HyperaiXConfigurationBuilder builder) =>
            builder.Use((evt, pvd, nxt) =>  
            {
                var logger = pvd.GetRequiredService<ILogger<HyperaiXConfigurationBuilder>>();
                logger.LogInformation("{}", evt);
                nxt(evt, pvd);
            });
    }
}