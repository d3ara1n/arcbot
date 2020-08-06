using Hyperai;
using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Services;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation;
using HyperaiShell.Foundation.ModelExtensions;
using HyperaiShell.Foundation.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Arcbot.Essential.Units
{
    public class VersionUnit : UnitBase
    {
        [Receive(MessageEventType.Group)]
        [Extract("!version")]
        public async Task GetVersion(Group group)
        {

            await group.SendPlainAsync(Summarize()); ;
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!version")]
        public async Task GetVersion(Friend friend)
        {
            await friend.SendPlainAsync(Summarize());
        }

        private string Summarize()
        {
            StringBuilder builder = new StringBuilder(": Version\n");
            builder.AppendLine($"CLR/{Assembly.GetAssembly(typeof(object)).GetName().Version}");
            builder.AppendLine($"Hyperai/{Assembly.GetAssembly(typeof(IApiClient)).GetName().Version}");
            builder.AppendLine($"Hyperai.Units/{Assembly.GetAssembly(typeof(UnitBase)).GetName().Version}");
            builder.AppendLine($"HyperaiShell/{Assembly.GetAssembly(typeof(PluginBase)).GetName().Version}");
            builder.AppendLine($"Arcbot/{Assembly.GetAssembly(typeof(VersionUnit)).GetName().Version}");
            return builder.ToString();
        }
    }
}