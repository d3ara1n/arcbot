namespace Arcbot.Essential.Models
{
    public class Record
    {
        public int Id { get; set; }
        public long Who { get; set; }
        public long Group { get; set; }
        public string Message { get; set; }
        public long TimeStamp { get; set; }
    }
}