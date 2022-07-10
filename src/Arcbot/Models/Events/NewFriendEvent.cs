using Onebot.Protocol.Models.Events;

namespace Arcbot.Models.Events;

public record NewFriendEvent: EventBase
{
    public long RequestId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Message { get; set; }
}