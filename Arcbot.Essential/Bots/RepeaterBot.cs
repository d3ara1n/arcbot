using System.Linq;
using Arcbot.Essential.Models;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Messages.ConcreteModels;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.ModelExtensions;
using Microsoft.Extensions.Logging;

namespace Arcbot.Essential.Bots
{
    public class RepeaterBot : BotBase
    {
        private ILogger _logger;

        public RepeaterBot(ILogger<RepeaterBot> logger)
        {
            _logger = logger;
        }

        public override void OnGroupMessage(object sender, GroupMessageEventArgs args)
        {
            var group = args.Group;
            var message = new MessageChain(args.Message.Where(x => !(x is Source or Quote))).Flatten();
            using (group.For(out GroupMessagePiece piece, () => new GroupMessagePiece(null)))
            {
                piece.Count++;
                if (piece.Message == message)
                {
                    if (piece.Count == 2)
                    {
                        group.SendAsync(args.Message.AsSendable());
                    }
                }
                else
                {
                    piece.Message = message;
                    piece.Count = 1;
                }
            }
        }
    }
}