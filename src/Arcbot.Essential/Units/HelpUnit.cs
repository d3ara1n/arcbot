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

        private string GetHelpText(MessageEventType type)
        {
            var entries = _service.GetEntries();
            StringBuilder builder = new();
            foreach (var ent in entries)
            {
                var desc = ent.Action.GetCustomAttribute<DescriptionAttribute>();
                var extr = ent.Action.GetCustomAttribute<ExtractAttribute>();
                if (desc == null || extr == null) continue;

                var rece = ent.Action.GetCustomAttribute<ReceiveAttribute>();
                
                if(rece?.Type != type) continue;

                var receStr = rece?.Type switch
                {
                    MessageEventType.Friend => "🧑‍🤝‍🧑",
                    MessageEventType.Group => "👪",
                    MessageEventType.Stranger => "👤",
                    _ => "👣"
                };
                builder.Append(receStr);
                builder.Append(' ');
                builder.AppendLine(extr.RawString);
                builder.Append(" - ");
                builder.AppendLine(desc.Description);
            }

            return builder.ToString().Trim();
        }

        [Receive(MessageEventType.Group)]
        [Extract("!help")]
        [Description("得到一份包含所有具有注释的群命令集合")]
        public async void Help(Group group)
        {
            await group.SendPlainAsync(GetHelpText(MessageEventType.Group));
        }
        
        [Receive(MessageEventType.Friend)]
        [Extract("!help")]
        [Description("得到一份包含所有具有注释的好友命令集合")]
        public async void Help(Friend friend)
        {
            await friend.SendPlainAsync(GetHelpText(MessageEventType.Friend));
        }
    }
}