using HyperaiX.Abstractions.Relations;
using Onebot.Protocol.Models.Relations;

namespace Arcbot.Extensions
{
    public static class GroupRoleExtensions
    {
        public static GroupRole ToRole(this Role role) => role switch
        {
            Role.Member => GroupRole.Member,
            Role.Owner => GroupRole.Owner,
            Role.Administrator => GroupRole.Administrator,
            
            _ => GroupRole.Member
        };

        public static Role ToRole(this GroupRole role) => role switch
        {
            GroupRole.Member => Role.Member,
            GroupRole.Administrator => Role.Administrator,
            GroupRole.Owner => Role.Owner,
            
            _ => Role.Member
        };
    }
}