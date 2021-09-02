using System;
using Hyperai.Events;
using Hyperai.Relations;

namespace Arcbot.Essential.Models
{
    public class ForwardChannel
    {
        public int Id { get; set; }
        public Signature Rule{ get; set; }
        public long Destination { get; set; }
        
        public MessageEventType DestinationType { get; set; }

        public ForwardChannel()
        {
            Rule = new Signature("*:*");
            Destination = 10000;
            DestinationType = MessageEventType.Friend;
        }

        public ForwardChannel(Signature rule, long destination, MessageEventType type)
        {
            Rule = rule;
            Destination = destination;
            DestinationType = type;
        }
    }
}