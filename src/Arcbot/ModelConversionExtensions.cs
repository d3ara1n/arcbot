using HyperaiX.Abstractions.Events;
using Onebot.Protocol.Models.Events;
using Onebot.Protocol.Models.Events.Meta;

namespace Arcbot;

public static class ModelConversionExtensions
{
    public static GenericEventArgs ToHyperai(this EventBase evt) => evt switch
    {
        UnknownEvent it => new UnknownEventArgs() { Data = it.RawObject },
        HeartbeatEvent it => new UnknownEventArgs() { Data = it },

        _ => new UnknownEventArgs() { Data = evt },
    };
}