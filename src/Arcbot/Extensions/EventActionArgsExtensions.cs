using System;
using HyperaiX;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Relations;
using Onebot.Protocol.Models.Actions;
using Onebot.Protocol.Models.Events;

namespace Arcbot.Extensions
{
    public static class EventActionArgsExtensions
    {
        public static GenericEventArgs ToEventArgs(this IEvent evt, IApiClient client) => evt switch
        {
            FriendMessageEvent it => new FriendMessageEventArgs()
            {
                Sender = new Friend()
                {
                    Identity = it.UserId,
                    Nickname = it.Sender.Nickname,
                    Remark = null
                },
                Message = it.Message.ToMessageChain()
            },
            GroupMessageEvent it => new GroupMessageEventArgs()
            {
                Group = client.GetGroupInfoAsync(it.GroupId).GetAwaiter().GetResult() ?? new Group() { Identity = it.GroupId },
                Message = it.Message.ToMessageChain(),
                Sender = new Member()
                {
                    DisplayName = it.Sender.Nickname,
                    GroupIdentity = it.GroupId,
                    Identity = it.UserId,
                    Role = it.Sender.Role.ToRole()
                },
            },
            HeartbeatEvent it => new UnknownEventArgs()
            {
                Data = it
            },
            ConnectEvent it => new UnknownEventArgs()
            {
                Data = it
            },
            UnknownEvent it => new UnknownEventArgs()
            {
                Data = it
            }
        };


        public static IAction ToAction(this GenericActionArgs action) => action switch
        {
            FriendMessageActionArgs it => new FriendMessageAction(it.FriendId, it.Message.ToMessage()),
            GroupMessageActionArgs it => new GroupMessageAction(it.GroupId, it.Message.ToMessage()),
            QueryGroupActionArgs it => new QueryGroupAction(it.GroupId),
            QueryMemberActionArgs it => new QueryMemberAction(it.GroupId, it.MemberId),
            QueryFriendActionArgs it => throw new NotImplementedException(),
            _ => null,
        };
    }
}