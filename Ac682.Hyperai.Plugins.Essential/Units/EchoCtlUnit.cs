using Ac682.Hyperai.Plugins.Essential.Services;
using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.ModelExtensions;
using System.Threading.Tasks;

namespace Ac682.Hyperai.Plugins.Essential.Units
{
    public class EchoCtlUnit : UnitBase
    {
        private readonly EchoService _service;

        public EchoCtlUnit(EchoService service)
        {
            _service = service;
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!echo.on")]
        public async Task EchoOn(Friend friend)
        {
            _service.On(friend.Identity);
            await friend.SendPlainAsync("你开启了回声模式。");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!echo.on")]
        [CheckTicket("echo.control")]
        public async Task EchoOn(Group group)
        {
            _service.On(group.Identity);
            await group.SendPlainAsync("群里开启了回声模式。");
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!echo.off")]
        public async Task EchoOff(Friend friend)
        {
            _service.Off(friend.Identity);
            await friend.SendPlainAsync("你关掉了回声模式。");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!echo.off")]
        [CheckTicket("echo.control")]
        public async Task EchoOff(Group group)
        {
            _service.Off(group.Identity);
            await group.SendPlainAsync("群里关闭了回声模式。");
        }
    }
}
