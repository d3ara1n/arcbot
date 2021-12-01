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
        [Handler("!reply {image:Image}")]
        public async Task ReplyImageAsync(Image image, MessageChain chain)
        {
            var builder = chain.CanBeReplied() ? chain.MakeReply() : new MessageChainBuilder();
            builder.Add(image);
            await Context.SendMessageAsync(builder.Build());
        }

        [Receiver(MessageEventType.Group | MessageEventType.Friend)]
        [Handler("{owner}/{repo}")]
        public Image Github(string owner, string repo)
        {
            return new Image(new Uri($"https://opengraph.githubassets.com/0/{owner}/{repo}"));
        }
    }
}