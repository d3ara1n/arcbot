using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HyperaiX;
using HyperaiX.Abstractions.Actions;
using HE = HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Clients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sora.Entities.Base;
using SE = Sora.EventArgs.SoraEvent;
using HyperaiX.Abstractions.Messages;
using SR = Sora.Entities;
using SM = Sora.Entities.Segment.DataModel;
using Sora.Entities.Segment;
using Sora.Enumeration;
using HM = HyperaiX.Abstractions.Messages.ConcreteModels;
using HR = HyperaiX.Abstractions.Relations;
using HyperaiX.Abstractions.Relations;
using Sora.Enumeration.EventParamsType;
using Sora.Entities.Info;
using System.Linq;
using Sora.Entities;

namespace Arcbot.Services
{
    public class OnebotClient : IApiClient
    {
        private readonly OnebotClientOptions _options;
        private readonly ILogger _logger;

        private Queue<SE.BaseSoraEventArgs> events = new();
        
        private SoraApi context;


        public OnebotClient(IOptions<OnebotClientOptions> options, ILogger<OnebotClient> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public HE.GenericEventArgs Read()
        {
            // server runs at single thread, no need thread safety
            while (events.Count == 0)
            {
                Thread.Sleep(300);
            }

            var evt = events.Dequeue();
            context = evt.SoraApi;
            return CastEvent(evt);
        }

        public GenericReceipt Write(GenericActionArgs action)
        {
            //TODO: do nothing
            return new GenericReceipt();
        }

        internal Task ForwardAsync(SE.BaseSoraEventArgs args)
        {
            events.Enqueue(args);
            return Task.CompletedTask;
        }

        private HE.GenericEventArgs CastEvent(SE.BaseSoraEventArgs args) => args switch
        {
            SE.GroupMessageEventArgs it => new HE.GroupMessageEventArgs()
            {
                Message = CastMessage(it.Message),
                Sender = CastMember(it.Sender.Id, it.SenderInfo),
                Group = ConstructGroup(it.SourceGroup.Id),
            },
            _ => new HE.UnknownEventArgs() // discord
        };

        private HR.GroupRole CastRole(MemberRoleType role)
        {
            return role switch
            {
                MemberRoleType.Owner => GroupRole.Owner,
                MemberRoleType.Admin => GroupRole.Administrator,
                _ => GroupRole.Member
            };
        }

        private HR.Member CastMember(long groupId, GroupSenderInfo info)
        {
            var member = new HR.Member()
            {
                Identity = info.UserId,
                GroupIdentity = groupId,
                Role = CastRole(info.Role),
                Title = info.Title,
                DisplayName = info.Nick,
            };
            return member;
        }

        private HR.Member CastMember(GroupMemberInfo info)
        {
            var member = new HR.Member()
            {
                Identity = info.UserId,
                GroupIdentity = info.GroupId,
                Role = CastRole(info.Role),
                Title = info.Title,
                DisplayName = info.Nick
            };
            return member;
        }

        private HR.Group ConstructGroup(long id)
        {
            var group = context.GetGroup(id);
            return new HR.Group()
            {
                Name = context.GetGroupInfo(id).GetAwaiter().GetResult().groupInfo.GroupName,
                Identity = id,
                Members = new Lazy<IEnumerable<HR.Member>>(() => group.GetGroupMemberList().GetAwaiter().GetResult().groupMemberList.Select(x => CastMember(x))),
                Owner = new Lazy<HR.Member>(() => CastMember(group.GetGroupMemberList().GetAwaiter().GetResult().groupMemberList.First(x => x.Role == MemberRoleType.Owner))),

            };
        }

        private MessageChain CastMessage(SR.MessageContext message)
        {
            if (message.IsForwardMessage())
            {
                var messageId = message.GetForwardMsgId();
                var forward = message.SoraApi.GetForwardMessage(messageId).GetAwaiter().GetResult();
                var builder = new MessageChainBuilder();
                foreach (var node in forward.nodeArray)
                {
                    builder.AddNode(node.Sender.Uid, node.Sender.Nick, CastMessageInternal(node.MessageBody));
                }
                return builder.Build();
            }
            {
                return CastMessageInternal(message.MessageBody);
            }
        }

        private MessageChain CastMessageInternal(MessageBody body)
        {
            var builder = new MessageChainBuilder();
            foreach (SoraSegment code in body)
            {
                builder.Add(code.Data switch
                {
                    SM.TextSegment it => new HM.Plain(it.Content),
                    SM.ImageSegment it => new HM.Image(new Uri(it.Url)),
                    SM.AtSegment it when it.Target == null => new HM.AtAll(),
                    SM.AtSegment it when it.Target != null => new HM.At(long.Parse(it.Target)),
                    SM.FaceSegment it => new HM.Face(it.Id),
                    SM.ReplySegment it => new HM.Quote(it.Target),
                    _ => null
                });
            }
            return builder.Build();
        }
    }
}