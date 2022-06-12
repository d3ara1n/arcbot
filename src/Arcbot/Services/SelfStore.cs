using System;
using System.Collections.Generic;
using System.Linq;
using HyperaiX.Abstractions.Relations;

namespace Arcbot.Services;

public class SelfStore
{
    private List<Group> cachedGroups = new();

    private List<Friend> cachedFriends = new();

    public Group FindGroup(long identity) =>
        cachedGroups.FirstOrDefault(x => x.Identity == identity);

    public Member FindMember(long groupId, long memberId) =>
        FindGroup(groupId)?.Members?.Value?.FirstOrDefault(x => x.Identity == memberId);

    public Friend FindFriend(long identity) =>
        cachedFriends.FirstOrDefault(x => x.Identity == identity);

    public void AddGroup(Group group) => 
        cachedGroups.Add(group);

    public void AddFriend(Friend friend) =>
        cachedFriends.Add(friend);

    public void Update(IEnumerable<Group> groups, IEnumerable<Friend> friends)
    {
        cachedGroups = groups.ToList();
        cachedFriends = friends.ToList();
    }
}