namespace Arcbot.Essential.Models.ProfileInventory
{
    public class Coin : Item
    {
        public Coin(long count)
        {
            Stack = count;
        }

        public Coin() : this(0) { }

        public override string DisplayName => "硬币";
    }
}