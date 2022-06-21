using System;
using System.Buffers.Text;
using System.IO;
using System.Threading.Tasks;
using Arcbot.Jobs;
using Arcbot.Services;
using HyperaiX;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.Logging;
using Onebot.Protocol;
using Quartz;
using Image = HyperaiX.Abstractions.Messages.ConcreteModels.Image;

namespace Arcbot.Units
{
    public class DebugUnit : UnitBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger _logger;

        public DebugUnit(ISchedulerFactory schedulerFactory, ILogger<DebugUnit> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Extract("!ping")]
        public string Ping()
        {
            return "pong!";
        }

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Extract("!version")]
        public MessageChain Version()
        {
            return MessageChain.Construct(new Plain("IDK"));
        }

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Regex(@"^http[s]?:\/\/github.com\/(?<owner>[a-zA-Z0-9_]+)\/(?<repo>[a-zA-Z0-9_]+)$")]
        public Image Github(string owner, string repo)
        {
            return new Image(new Uri($"https://opengraph.githubassets.com/0/{owner}/{repo}"));
        }

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Extract("!echo {chain}")]
        public MessageChain Echo(MessageChain chain)
        {
            return chain;
        }

        [Receiver(MessageEventType.Group)]
        [Extract("OMG{face:Face}")]
        public void Punch(MessageChain chain, Face face)
        {
            _logger.LogInformation("{}", chain.Flatten());
        }

        [Receiver(MessageEventType.Group)]
        public async Task Forward(MessageChain chain)
        {
            await Context.Client.SendGroupMessageAsync(594429092, chain);
        }

        [Receiver(MessageEventType.Group)]
        [Extract("!plus")]
        [Persistence(SharingScope.Member)]
        public string PlusOne(Session session)
        {
            var count = session.Get<int>("count", () => 0);
            count += 1;
            if (count > 2)
            {
                session.Set("count", 0);
            }
            else
            {
                session.Set("count", count);
            }

            return count.ToString();
        }
    }
}