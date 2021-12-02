using System;
using System.Buffers.Text;
using System.IO;
using System.Threading.Tasks;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Image = HyperaiX.Abstractions.Messages.ConcreteModels.Image;

namespace Arcbot.Units
{
    public class DebugUnit : UnitBase
    {

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Handler("!ping")]
        public string Ping()
        {
            return "pong!";
        }

        // 返回值可以是
        // string = MessageChain<Plain<string>>
        // MessageElement = MessageChain<MessageElement>
        // MessageChain
        // MessageChainBuilder
        // IEnumerable<MessageElement> = MessageChain
        // NOT SUPPORTED
        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Handler("!version")]
        public MessageChain Version()
        {
            return MessageChain.Construct(new Plain("IDK"));
        }

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [RegexAttribte(@"^http[s]?:\/\/github.com\/(?<owner>[a-zA-Z0-9_]+)\/(?<repo>[a-zA-Z0-9_]+)$")]
        public Image Github(string owner, string repo)
        {
            return new Image(new Uri($"https://opengraph.githubassets.com/0/{owner}/{repo}"));
        }

        [Receiver(MessageEventType.Group| MessageEventType.Friend)]
        [Handler("hal/{chain}")]
        public MessageChain Hal(MessageChain chain)
        {
            return chain;
        }
    }
}