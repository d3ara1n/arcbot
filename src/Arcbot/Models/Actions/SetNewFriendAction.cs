using System;
using Arcbot.Models.Receipts;
using Onebot.Protocol.Models.Actions;

namespace Arcbot.Models.Actions;

public record SetNewFriendAction : ActionBase
{
    protected override string Action => "set_new_friend";
    protected override Type Receipt => typeof(SetNewFriendReceipt);
    
    public string UserId { get; set; }
    public long RequestId { get; set; }
    public bool Accept { get; set; }
}