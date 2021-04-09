using System.ComponentModel;
using System.Reflection;
using System.Text;
using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.Essential.Units
{
    public class HelpUnit : UnitBase
    {
        private readonly IUnitService _service;

        public HelpUnit(IUnitService service)
        {
            _service = service;
        }

        private string GetHelpText()
        {
            var entries = _service.GetEntries();
            StringBuilder builder = new();
            foreach (var ent in entries)
            {
                var desc = ent.Action.GetCustomAttribute<DescriptionAttribute>();
                var extr = ent.Action.GetCustomAttribute<ExtractAttribute>();
                if (desc == null || extr == null) continue;

                var rece = ent.Action.GetCustomAttribute<ReceiveAttribute>();

                var receStr = rece.Type switch
                {
                    MessageEventType.Friend => "🧑‍🤝‍🧑",
                    MessageEventType.Group => "👪",
                    MessageEventType.Stranger => "👤",
                    _ => "👣"
                };

                StringBuilder sb = new();
                sb.Append(receStr);
                sb.Append(' ');
                sb.AppendLine(extr.RawString);
                sb.Append(" - ");
                sb.AppendLine(desc.Description);

                builder.AppendLine(sb.ToString());
            }

            return builder.ToString().Trim();
        }

        [Receive(MessageEventType.Group)]
        [Extract("!help")]
        [Description("得到一份包含所有具有注释的命令集合")]
        public async void Help(Group group)
        {
            await group.SendPlainAsync(GetHelpText());
        }
    }
}