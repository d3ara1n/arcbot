using System;

namespace Arcbot.Essential.Models.ProfileInventory
{
    public abstract class Item
    {
        public int Id { get; set; }
        public abstract string DisplayName { get; }
        public long Stack { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}