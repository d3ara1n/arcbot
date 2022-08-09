using Onebot.Protocol.Models.Events;

namespace Arcbot.Models.Events;

public record JoinGroupEvent : EventBase
{
    public long RequestId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string GroupId { get; set; }
    public string GroupName { get; set; }
    public string Message { get; set; }
    public bool Suspicious { get; set; }

    // optional
    public string InvitorId { get; set; }
    public string InvitorName { get; set; }
}