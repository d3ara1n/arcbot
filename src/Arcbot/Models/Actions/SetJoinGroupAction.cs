using System;
using Arcbot.Models.Receipts;
using Onebot.Protocol.Models.Actions;

namespace Arcbot.Models.Actions;

public record SetJoinGroupAction : ActionBase
{
    protected override string Action => "set_join_group";
    protected override Type Receipt => typeof(SetJoinGroupReceipt);

    public long RequestId { get; set; }
    public string UserId { get; set; }
    public string GroupId { get; set; }
    public bool Accept { get; set; }

    // optional
    public bool Block { get; set; }
    public string Message { get; set; }
}