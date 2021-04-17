using System;
using Hyperai.Messages;

namespace Arcbot.Essential.Models
{
    public class GroupReplyPiece
    {
        public Guid Id { get; set; }
        public long GroupId { get; set; }
        public string Trigger { get; set; }
        public string Reply { get; set; }
        public long OperatorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}