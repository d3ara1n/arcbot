using System.Collections.Concurrent;
using Arcbot.Essential.Models.ProfileInventory;

namespace Arcbot.Essential.Models
{
    public class Profile
    {
        public Profile(long userAttachedTo)
        {
            UserAttachedTo = userAttachedTo;
        }

        public int Id { get; set; }
        public long UserAttachedTo { get; set; }
        public ConcurrentBag<Item> Inventory { get; set; } = new();
    }
}