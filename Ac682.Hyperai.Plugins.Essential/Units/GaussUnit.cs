using System;
using System.Collections.Generic;
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

namespace Ac682.Hyperai.Plugins.Essential.Units
{
    public class GaussUnit : UnitBase
    {
        private readonly IEnumerable<IConfigurationSection> _sections;
        private readonly int _count;
        private readonly Random _random;
        public GaussUnit(IPluginConfiguration<PluginEntry> configuration)
        {
            _sections = configuration.Value.GetSection("Gauss").GetChildren();
            _count = _sections.Count();
            _random = new Random();
        }

        [Receive(MessageEventType.Group)]
        [CheckTicket("iamgauss")]
        [Extract("!gauss {who}")]
        public async Task Write(long who, Group group, IApiClient client)
        {
            var member = await client.RequestAsync<Member>(new Member() { Identity = who, Group = new Lazy<Group>(group) });
            await Next(member.DisplayName, group);
        }

        [Receive(MessageEventType.Group)]
        [Extract("[hyper.at({who})] nb")]
        public async Task At(long who, Group group, IApiClient client)
        {
            await Write(who, group, client);
        }

        [Receive(MessageEventType.Group)]
        [Extract("来点{who}笑话")]
        public async Task Next(string who, Group group)
        {
            var ind = _random.Next(_count);
            var sel = _sections.Skip(ind).First().Value;
            await group.SendPlainAsync(string.Format(sel, who));
        }
    }
}