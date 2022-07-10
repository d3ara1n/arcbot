using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;
using Microsoft.Extensions.Caching.Memory;
using Onebot.Protocol;

namespace Arcbot;

public static class OnebotClientExtensions
{
    private static readonly TimeSpan EXPIRATION = TimeSpan.FromMinutes(30);

    public static async Task<Group> GetHyperaiGroupAsync(this OnebotClient client, long id, IMemoryCache cache)
    {
        return await cache.GetOrCreateAsync($"{id}:_", async entry =>
        {
            entry.SlidingExpiration = EXPIRATION;

            var groupName = (await client.GetGroupInfoAsync(id.ToString())).GroupName;

            var group = new Group
            {
                Identity = id,
                Name = groupName,
                Members = new Lazy<IEnumerable<Member>>(() => client.GetHyperaiGroupMembersAsync(id, cache).Result)
            };

            return group;
        });
    }

    public static async Task<IEnumerable<Member>> GetHyperaiGroupMembersAsync(this OnebotClient client, long id,
        IMemoryCache cache)
    {
        return await cache.GetOrCreateAsync($"{id}:*", async entry =>
        {
            entry.SlidingExpiration = EXPIRATION;

            var list = await client.GetGroupMemberListAsync(id.ToString());
            return list.Select(x => new Member
            {
                DisplayName = x.Nickname,
                Nickname = x.Nickname,
                GroupIdentity = id,
                Identity = long.Parse(x.UserId),
                Role = GroupRole.Member
            });
        });
    }

    public static async Task<Member> GetHyperaiMemberAsync(this OnebotClient client, long groupId, long memberId,
        IMemoryCache cache)
    {
        return await cache.GetOrCreateAsync($"{groupId}:{memberId}", async entry =>
        {
            entry.SlidingExpiration = EXPIRATION;

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
        });
    }

    public static async Task<Friend>
        GetHyperaiFriendAsync(this OnebotClient client, long friendId, IMemoryCache cache)
    {
        return await cache.GetOrCreateAsync($"_:{friendId}", async entry =>
        {
            entry.SlidingExpiration = EXPIRATION;

            var receipt = await client.GetUserInfoAsync(friendId.ToString());
            var friend = new Friend
            {
                Identity = friendId,
                Nickname = receipt.Nickname,
                Remark = receipt.Nickname
            };

            return friend;
        });
    }

    public static async Task<Self> GetHyperaiSelfAsync(this OnebotClient client, IMemoryCache cache)
    {
        return await cache.GetOrCreateAsync("_:_", async entry =>
        {
            entry.SlidingExpiration = EXPIRATION;

            var tasks = (client.GetSelfInfoAsync(), client.GetGroupListAsync(), client.GetFriendListAsync());

            var nickname = await tasks.Item1;
            var groups = await tasks.Item2;
            var friends = await tasks.Item3;

            var self = new Self
            {
                Identity = long.Parse(nickname.UserId),
                Nickname = nickname.Nickname,
                Groups = new Lazy<IEnumerable<Group>>(() =>
                    groups.Select(x => client.GetHyperaiGroupAsync(long.Parse(x.GroupId), cache).Result)),
                Friends = new Lazy<IEnumerable<Friend>>(() =>
                    friends.Select(x => client.GetHyperaiFriendAsync(long.Parse(x.UserId), cache).Result))
            };

            return self;
        });
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