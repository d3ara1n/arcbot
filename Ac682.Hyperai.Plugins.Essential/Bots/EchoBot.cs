using Ac682.Hyperai.Plugins.Essential.Services;
using Hyperai.Messages;
using Hyperai.Relations;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.ModelExtensions;

namespace Ac682.Hyperai.Plugins.Essential.Bots
{
    public class EchoBot : BotBase
    {
        private readonly EchoService _service;
        public EchoBot(EchoService service)
        {
            _service = service;
        }
        public override void OnFriendMessage(Friend friend, MessageChain message)
        {
            if (_service.IsOn(friend.Identity)) friend.SendAsync(message.AsReadable()).Wait();
        }

        public override void OnGroupMessage(Member member, Group group, MessageChain message)
        {
            if (_service.IsOn(group.Identity)) group.SendAsync(message.AsReadable()).Wait();
        }
    }
}
