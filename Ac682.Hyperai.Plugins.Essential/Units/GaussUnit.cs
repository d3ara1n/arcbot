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
using Microsoft.Extensions.Configuration;

namespace Ac682.Hyperai.Plugins.Essential.Units
{
    public class GaussUnit : UnitBase
    {
        private readonly IEnumerable<IConfigurationSection> _sections;
        private readonly int _count;
        private readonly Random _random;
        public GaussUnit(IConfiguration configuration)
        {
            _sections = configuration.GetSection("Gauss").GetChildren();
            _count = _sections.Count();
            _random = new Random();
        }

        [Receive(MessageEventType.Group)]
        [CheckTicket("iamgauss")]
        [Extract("!gauss {who}")]
        public async Task Write(long who, Group group, IApiClient client)
        {
            var member = await client.RequestAsync<Member>(new Member(){Identity = who, Group = new Lazy<Group>(group)});
            var ind = _random.Next(_count);
            var sel = _sections.Skip(ind).First().Value;
            await group.SendPlainAsync(string.Format(sel, member.DisplayName));
        }
    }
}