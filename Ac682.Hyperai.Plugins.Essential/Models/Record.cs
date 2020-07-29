using System;
using System.Collections.Generic;
using System.Text;

namespace Ac682.Hyperai.Plugins.Essential.Models
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
