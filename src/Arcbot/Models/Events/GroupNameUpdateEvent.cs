using Onebot.Protocol.Models.Events;

namespace Arcbot.Models.Events;

public record GroupNameUpdateEvent : EventBase
{
    public string GroupId { get; set; }
    public string GroupName { get; set; }
    public string OperatorId { get; set; }
}