namespace Arcbot.Essential.Models
{
    public class SetuWhite
    {
        public bool IsOn { get; set; }

        public SetuWhite(bool on) => IsOn = on;
        public SetuWhite() => IsOn = false;
    }
}