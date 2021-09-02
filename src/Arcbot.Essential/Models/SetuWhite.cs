namespace Arcbot.Essential.Models
{
    public class SetuWhite
    {
        public SetuWhite(bool on)
        {
            IsOn = on;
        }

        public SetuWhite() : this(false)
        {
        }

        public bool IsOn { get; set; }

        public bool SexyMode { get; set; }
    }
}