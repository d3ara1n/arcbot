using System;
using Arcbot.Models.Receipts;
using Onebot.Protocol.Models.Actions;

namespace Arcbot.Models.Actions;

public record GetMessageAction : ActionBase
{
    protected override string Action => "get_message";
    protected override Type Receipt => typeof(GetMessageReceipt);
    
    public string MessageId { get; set; }
}