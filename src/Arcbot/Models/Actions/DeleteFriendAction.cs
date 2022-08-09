using System;
using Arcbot.Models.Receipts;
using Onebot.Protocol.Models.Actions;

namespace Arcbot.Models.Actions;

public record DeleteFriendAction : ActionBase
{
    protected override string Action => "delete_friend";
    protected override Type Receipt => typeof(DeleteFriendReceipt);

    public string UserId { get; set; }
}