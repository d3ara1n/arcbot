using System.Threading.Tasks;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;

namespace Arcbot.Units
{
    public class DebugUnit : UnitBase
    {

        [Receiver(MessageEventType.Group)]
        [Handler("!ping")]
        public async Task PingAsync(Group group)
        {
            await SendGroupMessageAsync(group.Identity, MessageChain.Construct(new Plain("pong!")));
        }

        [Receiver(MessageEventType.Friend)]
        [Command("echo")]
        [Option("message", 'm', "message")]
        public async Task EchoAsync(Friend friend, MessageChain message)
        {
            await SendFriendMessageAsync(friend.Identity, message);
        }

        // 返回值可以是
        // string = MessageChain<Plain<string>>
        // MessageElement = MessageChain<MessageElement>
        // MessageChain
        // MessageChainBuilder
        // IEnumerable<MessageElement> = MessageChain
        // NOT SUPPORTED
        [Receiver(MessageEventType.Group)]
        [Command("version")]
        public MessageChain Version()
        {
            return MessageChain.Construct(new Plain("IDK"));
        }
        
        [Receiver(MessageEventType.Group)]
        [Handler("!reply {image:Image}")]
        public async Task ReplyImageAsync(Group group, Image image, MessageChain chain)
        {
            var builder = chain.CanBeReplied() ? chain.MakeReply() : new MessageChainBuilder();
            builder.Add(image);
            await SendGroupMessageAsync(group.Identity, builder.Build());
        }
    }
}