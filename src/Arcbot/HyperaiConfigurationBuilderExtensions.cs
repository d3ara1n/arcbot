

using HyperaiX.Abstractions.Events;
using HyperaiX.Clients;

namespace Arcbot
{
    public static class HyperaiConfigurationBuilderExtensions
    {
        public static HyperaiXConfigurationBuilder UseUnits(this HyperaiXConfigurationBuilder builder) =>builder
            .Use((evt, pvd, nxt) =>
            {
                nxt(evt, pvd);
            });

        public static HyperaiXConfigurationBuilder UseEventBlocker(this HyperaiXConfigurationBuilder builder) => builder
            .Use((evt, pvd, nxt) =>
            {
                if (evt is not UnknownEventArgs) nxt(evt, pvd);
            });
    }
}