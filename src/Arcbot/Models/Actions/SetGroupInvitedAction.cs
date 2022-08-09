using System;
using Arcbot.Models.Receipts;
using Onebot.Protocol.Models.Actions;

namespace Arcbot.Models.Actions;

public record SetGroupInvitedAction : ActionBase
{
    protected override string Action => "set_group_invited";
    protected override Type Receipt => typeof(SetGroupInvitedReceipt);

    public long RequestId { get; set; }
    public string GroupId { get; set; }
    public bool Accept { get; set; }
}