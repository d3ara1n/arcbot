using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Arcbot.Essential.Models;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Messages.ConcreteModels;
using Hyperai.Relations;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.ModelExtensions;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.Logging;

namespace Arcbot.Essential.Bots
{
    public class ForwardBot : BotBase
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public ForwardBot(IPluginRepository<PluginEntry> repository, ILogger<ForwardBot> logger)
        {
            _repository = repository.Value;
            _logger = logger;
        }

        public override void OnFriendMessage(object sender, FriendMessageEventArgs args)
        {
            var list = _repository.Query<ForwardChannel>().ToList();
            foreach (var ele in list)
            {
                if (ele.Rule.Match(args.User))
                {
                    var chain = new MessageChain(
                        args.Message.Prepend(new Plain($"{args.User.Nickname}({args.User.Identity}):\n------\n")));
                    Send(ele, chain);
                }
            }
        }

        public override void OnGroupMessage(object sender, GroupMessageEventArgs args)
        {
            var list = _repository.Query<ForwardChannel>().ToList();
            foreach (var ele in list)
            {
                if (ele.Rule.Match(args.User))
                {
                    var chain = new MessageChain(args.Message.Prepend(new Plain(
                        $"[{args.Group.Name}({args.Group.Identity})]{args.User.DisplayName}({args.User.Identity}):\n------\n")));
                    Send(ele, chain);
                }
            }
        }

        private void Send(ForwardChannel channel, MessageChain chain)
        {
            _logger.LogInformation($"Forward to {channel.DestinationType switch{ MessageEventType.Friend => "f", MessageEventType.Group => "g" } }{channel.Destination} with rule {channel.Rule.Expression}:\n{chain.ToString()}");
            switch (channel.DestinationType)
            {
                case MessageEventType.Friend:
                    var friend = new Friend() {Identity = channel.Destination};
                    friend.SendAsync(chain).Wait();
                    break;
                case MessageEventType.Group:
                    var group = new Group() {Identity = channel.Destination};
                    group.SendAsync(chain).Wait();
                    break;
            }
        }
    }
}