using System.Collections.Generic;

namespace Arcbot.Essential.Models
{
    public class SetuArtwork
    {
        public string Title { get; set; }
        public long Artwork { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public string Original { get; set; }
    }
}