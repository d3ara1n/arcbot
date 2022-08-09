using Onebot.Protocol.Models.Events;

namespace Arcbot.Models.Events;

public record FriendPokeEvent : EventBase
{
    public string UserId { get; set; }
    public string ReceiverId { get; set; }
}