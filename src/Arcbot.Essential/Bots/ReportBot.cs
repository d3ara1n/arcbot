using System;
using System.Linq;
using System.Threading.Tasks;
using Arcbot.Essential.Services;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Messages.ConcreteModels;
using Hyperai.Relations;
using Hyperai.Services;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.ModelExtensions;
using Microsoft.Extensions.Configuration;

namespace Arcbot.Essential.Bots
{
    public class ReportBot: BotBase
    {
        private readonly IApiClient _client;
        private readonly ReportService _service;

        public ReportBot(ReportService service, IApiClient client)
        {
            _service = service;
            _client = client;
        }

        public override void OnFriendRecall(object client, FriendRecallEventArgs args) =>
        _service.Report(builder => builder
                .AddPlain($"好友 {args.WhoseMessage.Nickname}({args.WhoseMessage.Identity}) 的消息撤回:\n=========\n")
                .AddRange(_client.RequestAsync(MessageChain.Construct(new Source(args.MessageId)))
                    .GetAwaiter().GetResult()));


        public override void OnFriendMessage(object sender, FriendMessageEventArgs args) =>
            _service.Report(builder => builder
                .AddPlain($"好友 {args.User.Nickname}({args.User.Identity}) 发送消息:\n========\n")
                .AddRange(args.Message));

        public override void OnGroupMemberMuted(object sender, GroupMemberMutedEventArgs args)
        {
            if (args.Whom.Identity == Me.Identity)
            {
                _service.Report(builder => builder.AddPlain($"我在群 {args.Group.Name} 被禁言了 {args.Duration}"));
            }
        }

        public override void OnGroupPermissionChanged(object sender, GroupPermissionChangedEventArgs args)
        {
            if (args.Whom.Identity == Me.Identity)
            {
                _service.Report(builder => builder
                    .AddPlain($"我在群 {args.Group.Name} 被提为 {args.Present}"));
            }
        }

        public override void OnGroupLeft(object sender, GroupLeftEventArgs args)
        {
            if (args.Who.Identity == Me.Identity)
            {
                _service.Report(builder => builder
                    .AddPlain($"我在群 {args.Group.Name} 被 {args.Operator.DisplayName} 踢了"));
            }
        }

        public override void OnGroupRecall(object sender, GroupRecallEventArgs args) =>
            _service.Report(builder => builder
                .AddPlain(
                    $"群 {args.Group.Name}({args.Group.Identity}) 中 {args.WhoseMessage.Nickname}({args.WhoseMessage.Identity}) 撤回了\n========\n")
                .AddRange(_client.RequestAsync(MessageChain.Construct(new Source(args.MessageId))).GetAwaiter()
                    .GetResult()));


        public override void OnGroupMessage(object sender, GroupMessageEventArgs args)
        {
            // 转发闪照
            if(args.Message.Any(x=>x is Flash))
            {
                var flash = args.Message.First( x=> x is Flash) as Flash;
                _service.Report(builder => builder
                .AddPlain($"群 {args.Group.Name} 中 {args.User.DisplayName} 发送闪照:\n")
                .AddImage(flash.ImageId, flash.Source));
            }
        }

        public override void OnEverything(object sender, GenericEventArgs args)
        {
            base.OnEverything(sender, args);
            if (args is FriendRequestEventArgs fr)
            {
                var response = new FriendResponseEventArgs()
                {
                    Flag = fr.Flag,
                    Operation = FriendResponseEventArgs.ResponseOperation.Approve
                };
                
                _client.SendAsync(response);
                _service.Report(builder => builder.AddPlain($"{fr.Who} 申请添加好友 ({fr.Comment}), 已通过"));
            }

            if(args is GroupRequestEventArgs gr)
            {
                var response = new GroupResponseEventArgs()
                {
                    Flag = gr.Flag,
                    Operation = GroupResponseEventArgs.ResponseOperation.Approve
                };
                _service.Report(builder => builder.AddPlain($"我被拉入群 {gr.GroupId}, 已通过"));
                _client.SendAsync(response);
            }
        }


    }
}