using System;
using System.Buffers.Text;
using System.IO;
using System.Threading.Tasks;
using Arcbot.Jobs;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Quartz;
using Image = HyperaiX.Abstractions.Messages.ConcreteModels.Image;

namespace Arcbot.Units
{
    public class DebugUnit : UnitBase
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public DebugUnit(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Handler("!ping")]
        public string Ping()
        {
            var job = JobBuilder.Create<HelloJob>()
                .WithIdentity("SayHello")
                .UsingJobData("IsGroup", Context.Group != null)
                .UsingJobData("Group", Context.Group != null ? Context.Group.Identity : 0)
                .UsingJobData("User", Context.Sender.Identity)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("PingTrigger")
                .StartAt(DateTime.Now + TimeSpan.FromSeconds(15))
                .Build();


            _schedulerFactory.GetScheduler().GetAwaiter().GetResult().ScheduleJob(job, trigger);
            return "pong!";
        }
        
        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Handler("!version")]
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
        [Handler("!echo {chain}")]
        public MessageChain Echo(MessageChain chain)
        {
            return chain;
        }
    }
}