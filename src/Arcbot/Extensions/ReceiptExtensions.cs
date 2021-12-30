using System;
using System.Collections.Generic;
using HyperaiX.Abstractions.Relations;
using Onebot.Protocol.Models;
using Spectre.Console;
using H = HyperaiX.Abstractions.Receipts;
using O = Onebot.Protocol.Models.Receipts;

namespace Arcbot.Extensions
{
    public static class ReceiptExtensions
    {
        public static H.GenericReceipt ToReceipt(this O.IReceipt receipt) => receipt switch
        {
            O.GenericReceipt it => new H.GenericReceipt(),
            O.MessageReceipt it => new H.MessageReceipt()
            {
                MessageId = it.MessageId
            },
            O.QueryGroupReceipt it => new H.QueryGroupReceipt()
            {
                Group = new()
                {
                    Identity = it.GroupId,
                    Name = it.GroupName,
                    Members = new Lazy<IEnumerable<Member>>(() => null),
                    Owner = new Lazy<Member>( () => null)
                },
            },
            O.QueryMemberReceipt it => new H.QueryMemberReceipt()
            {
                Group = new()
                {
                    Identity = it.GroupId,
                    Name = null,
                    Members = new Lazy<IEnumerable<Member>>(() => null),
                    Owner = new Lazy<Member>(()=>null)
                },
                Member = new()
                {
                    DisplayName = it.Card,
                    Group = new Lazy<Group>(() => null),
                    GroupIdentity = it.GroupId,
                    Identity = it.UserId,
                    IsMuted = false,
                    Nickname = it.Nickname,
                    Role = GroupRole.Member,
                    Title = it.Title
                }
            },
            null => null
        };
    }
}