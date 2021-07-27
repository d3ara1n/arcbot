using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Arcbot.Essential.Models;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Messages.ConcreteModels;
using Hyperai.Relations;
using Hyperai.Services;
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
        private readonly IApiClient _client;

        public ForwardBot(IPluginRepository<PluginEntry> repository, ILogger<ForwardBot> logger, IApiClient client)
        {
            _repository = repository.Value;
            _logger = logger;
            _client = client;
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
                    Send(ele, new MessageChain(args.Message.Prepend(new Plain($"[{args.Group.Name}({args.Group.Identity})]{args.User.DisplayName}({args.User.Identity}):\n------\n"))));
                }
            }
        }

        private void AddToChainBuilder(MessageChainBuilder builder, MessageChain from)
        {
            foreach (var ele in from)
            {
                builder.Add(ele);
            }
        }

        private void Send(ForwardChannel channel, MessageChain chain)
        {
            _logger.LogInformation("Forward to {}{} with rule {}.",channel.DestinationType switch{ MessageEventType.Friend => "f", MessageEventType.Group => "g", _ => "_" },channel.Destination,channel.Rule.Expression);
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