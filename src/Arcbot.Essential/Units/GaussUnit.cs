using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Services;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.ModelExtensions;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.Configuration;

namespace Arcbot.Essential.Units
{
    public class GaussUnit : UnitBase
    {
        private readonly int _count;
        private readonly Random _random;
        private readonly IEnumerable<IConfigurationSection> _sections;

        public GaussUnit(IPluginConfiguration<PluginEntry> configuration)
        {
            _sections = configuration.Value.GetSection("Gauss").GetChildren();
            _count = _sections.Count();
            _random = new Random();
        }

        [Receive(MessageEventType.Group)]
        [RequiredTicket("iamgauss")]
        [Extract("!gauss {who}")]
        public async Task Write(long who, Group group, IApiClient client)
        {
            var member = await client.RequestAsync(new Member {Identity = who, Group = new Lazy<Group>(group)});
            await Next(member.DisplayName, group);
        }

        [Receive(MessageEventType.Group)]
        [Extract("[hyper.at({who})] nb")]
        [Description("当某人很牛逼时，at他并加一个nb")]
        public async Task At(long who, Group group, IApiClient client)
        {
            await Write(who, group, client);
        }

        [Receive(MessageEventType.Group)]
        [Extract("来点{who}笑话")]
        [Description("牛逼的笑话")]
        public async Task Next(string who, Group group)
        {
            var ind = _random.Next(_count);
            var sel = _sections.Skip(ind).First().Value;
            await group.SendAsync(string.Format(sel, who).MakeMessageChain());
        }
    }
}