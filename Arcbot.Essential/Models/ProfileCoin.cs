namespace Arcbot.Essential.Models
{
    public class ProfileCoin : IProfileField
    {
        public long Coin { get; set; }

        public void Take(int count)
        {
            Coin -= count;
        }

        public void Put(int count)
        {
            Coin += count;
        }
    }
}