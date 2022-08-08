using System;
using System.Linq;
using Arcbot.Modules.Repeater.Options;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.Options;

namespace Arcbot.Modules.Repeater.Units;

public class RepeaterUnit : UnitBase
{
    private readonly RepeaterOptions _options;

    private readonly Random rand = new();

    public RepeaterUnit(IOptions<RepeaterOptions> options)
    {
        _options = options.Value;
    }

    [Receiver(MessageEventType.Group)]
    public void Repeat(MessageChain chain, Group group)
    {
        if (_options.Enabled && _options.ActivatedGroups != null && _options.ActivatedGroups.Contains(group.Identity) &&
            chain.All(x => x is Plain or Image))
            if (rand.Next(100) == 0)
                Context.SendAsync(chain).Wait();
    }
}