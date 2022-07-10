using Onebot.Protocol.Models.Events;

namespace Arcbot.Models.Events;

public record GroupInvitedEvent: EventBase
{
    public long RequestId { get; set; }
    public string GroupId { get; set; }
    public string GroupName { get; set; }
    
    // optional
    public string InvitorId { get; set; }
    public string InvitorName { get; set; }
}