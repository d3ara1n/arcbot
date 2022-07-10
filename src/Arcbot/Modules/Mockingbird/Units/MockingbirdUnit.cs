using System;
using System.Linq;
using Arcbot.Data;
using Arcbot.Modules.Mockingbird.Options;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.Options;

namespace Arcbot.Modules.Mockingbird.Units;

public class MockingbirdUnit : UnitBase
{
    private static readonly object locker = new();
    private readonly ArcContext _context;
    private readonly MockingbirdOptions _options;

    private readonly Random rnd = new();

    public MockingbirdUnit(ArcContext context, IOptions<MockingbirdOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    [Receiver(MessageEventType.Group)]
    [Persistence(SharingScope.Group)]
    public void Capture(MessageChain chain, Group group, Session session)
    {
        if (!(_options.Enabled && _options.ActivatedGroups.Contains(group.Identity))) return;
        if (chain.Any(x => x is not Plain)) return;
        EnsureCreated(session);
        var message = chain.ToString();
        var last = Peek(session);
        if (last == message) return;

        var found = false;
        Enumerate(session, str =>
        {
            if (found)
            {
                if (last == str)
                {
                    var model = new TriggerModel
                    {
                        Group = group.Identity,
                        Keyword = last,
                        Response = message
                    };

                    _context.Triggers.Add(model);
                    return true;
                }
            }
            else
            {
                if (message == str) found = true;
            }

            return false;
        });
        Push(session, message);
    }

    [Receiver(MessageEventType.Group)]
    public async void Trigger(MessageChain chain, Group group)
    {
        if (!(_options.Enabled && _options.ActivatedGroups.Contains(group.Identity))) return;
        if (chain.Any(x => x is not Plain)) return;
        var message = chain.ToString();
        lock (locker)
        {
            var results = _context.Triggers.Where(x => x.Keyword == message).ToList();
            if (results.Count > 0)
            {
                var choice = rnd.Next(results.Count);
                Context.SendAsync(MessageChain.Construct(new Plain(results[choice].Response))).Wait();
            }
        }
    }

    private void EnsureCreated(Session session)
    {
        if (!session.Data.ContainsKey("_"))
        {
            session.Data.Add("_", new string[20]);
            session.Data.Add(".", 0);
            session.Data.Add("!", 20);
        }
    }

    private string Peek(Session session)
    {
        var array = session.Get<string[]>("_");
        var index = session.Get<int>(".");

        return array[index];
    }

    private void Push(Session session, string str)
    {
        var array = session.Get<string[]>("_");
        var index = session.Get<int>(".");
        var max = session.Get<int>("!");

        if (array[index] == null)
        {
            array[index] = str;
        }
        else
        {
            index = (index + 1) % max;
            array[index] = str;
            session.Set(".", index);
        }
    }

    private void Enumerate(Session session, Func<string, bool> action)
    {
        var array = session.Get<string[]>("_");
        var index = session.Get<int>(".");
        var max = session.Get<int>("!");

        for (var i = 0; i < max; i++)
        {
            var val = array[index];
            if (val == null) break;
            if (action(val)) return;
            index = (index + max - 1) % max;
        }
    }
}