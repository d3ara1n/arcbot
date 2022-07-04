using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;
using Onebot.Protocol;

namespace Arcbot;

public static class OnebotClientExtensions
{
    public static async Task<Group> GetHyperaiGroupAsync(this OnebotClient client, long id)
    {
        var groupName = (await client.GetGroupInfoAsync(id.ToString())).GroupName;

        var group = new Group
        {
            Identity = id,
            Name = groupName,
            Members = new Lazy<IEnumerable<Member>>(() =>
            {
                var list = client.GetGroupMemberListAsync(id.ToString()).Result;
                return list.Select(x => new Member
                {
                    DisplayName = x.Nickname,
                    Nickname = x.Nickname,
                    GroupIdentity = id,
                    Identity = long.Parse(x.UserId),
                    Role = GroupRole.Member
                });
            })
        };

        return group;
    }

    public static async Task<Member> GetHyperaiMemberAsync(this OnebotClient client, long groupId, long memberId)
    {
        var receipt = await client.GetGroupMemberInfoAsync(groupId.ToString(), memberId.ToString());

        var member = new Member
        {
            Identity = long.Parse(receipt.UserId),
            DisplayName = receipt.Nickname,
            Nickname = receipt.Nickname,
            GroupIdentity = groupId,
            Role = GroupRole.Member,
            Title = null,
            IsMuted = false
        };

        return member;
    }

    public static async Task<string> SendHyperaiFriendMessageAsync(this OnebotClient client, long friendId,
        MessageChain chain)
    {
        var receipt = await client.SendPrivateMessageAsync(friendId.ToString(), chain.ToOnebot(client));

        var messageId = receipt.MessageId;

        return messageId;
    }

    public static async Task<string> SendHyperaiGroupMessageAsync(this OnebotClient client, long groupId,
        MessageChain chain)
    {
        var receipt = await client.SendGroupMessageAsync(groupId.ToString(), chain.ToOnebot(client));

        var messageId = receipt.MessageId;

        return messageId;
    }
}