using Ac682.Hyperai.Plugins.Essential.Models;
using Ac682.Hyperai.Plugins.Essential.Services;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Services;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.ModelExtensions;
using HyperaiShell.Foundation.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ac682.Hyperai.Plugins.Essential.Units
{
    public class RecordCtlUnit : UnitBase
    {
        private readonly RecordService _service;
        private readonly IApiClient _client;

        public RecordCtlUnit(RecordService service, IApiClient client)
        {
            _service = service;
            _client = client;
        }

        [Receive(MessageEventType.Group)]
        [Extract("!record.on")]
        [CheckTicket("record.control.on")]
        public async Task TurnOnRecoding(Group group)
        {
            GroupRecordState state;
            using (group.For(out state, () => new GroupRecordState()))
            {
                state.IsOn = true;
                await group.SendPlainAsync("Recording.");
            }
        }

        [Receive(MessageEventType.Group)]
        [Extract("!record.off")]
        [CheckTicket("record.control.off")]
        public async Task TurnOffRecoding(Group group)
        {
            GroupRecordState state;
            using (group.For(out state, () => new GroupRecordState()))
            {
                state.IsOn = false;
                await group.SendPlainAsync("Recording ended.");
            }
        }

        [Receive(MessageEventType.Group)]
        [Extract("!record.state")]
        [CheckTicket("record.state")]
        public async Task IsRecoding(Group group)
        {
            GroupRecordState state = group.Retrieve(() => new GroupRecordState());
            await group.SendPlainAsync($"Is {state.IsOn switch { true => "On", false => "Off" }}");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!record.rank")]
        [CheckTicket("record.rank")]
        public async Task QueryRecords(Group group)
        {
            var records = _service.Rank(group);
            int n = records.Count();
            if (n > 0)
            {
                var builder = new StringBuilder($"{group.Name} ranks: ");
                for (int i = 0; i < n; i++)
                {
                    var member = new Member()
                    {
                        Identity = records[i].Item1,
                        Group = new Lazy<Group>(group)
                    };
                    member = await _client.RequestAsync(member);
                    builder.Append($"\n[{i}]{member.DisplayName}({member.Identity}): {records[i].Item2}");
                }
                await group.SendPlainAsync(builder.ToString());
            }
            else
            {
                await group.SendPlainAsync("No records.");
            }
        }
    }
}
