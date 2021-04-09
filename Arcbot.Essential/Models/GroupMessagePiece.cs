namespace Arcbot.Essential.Models
{
    public class GroupMessagePiece
    {
        public string Message { get; set; }
        public int Count { get; set; }

        public GroupMessagePiece(string message)
        {
            Message = message;
        }
    }
}