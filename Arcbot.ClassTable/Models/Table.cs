using System.Collections.Generic;

namespace Arcbot.ClassTable.Models
{
    public class Table
    {
        public long ContactIds { get; set; }
        public IList<Class> Classes { get; set; }
    }
}