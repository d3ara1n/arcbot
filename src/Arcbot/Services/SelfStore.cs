using System.Collections.Generic;
using System.Linq;
using HyperaiX.Abstractions.Relations;

namespace Arcbot.Services;

public class SelfStore
{
    private List<Friend> cachedFriends = new();
    private List<Group> cachedGroups = new();

    public Group FindGroup(long identity)
    {
        return cachedGroups.FirstOrDefault(x => x.Identity == identity);
    }

    public Member FindMember(long groupId, long memberId)
    {
        return FindGroup(groupId)?.Members?.Value?.FirstOrDefault(x => x.Identity == memberId);
    }

    public Friend FindFriend(long identity)
    {
        return cachedFriends.FirstOrDefault(x => x.Identity == identity);
    }

    public void AddGroup(Group group)
    {
        cachedGroups.Add(group);
    }

    public void AddFriend(Friend friend)
    {
        cachedFriends.Add(friend);
    }

    public void Update(IEnumerable<Group> groups, IEnumerable<Friend> friends)
    {
        cachedGroups = groups.ToList();
        cachedFriends = friends.ToList();
    }
}