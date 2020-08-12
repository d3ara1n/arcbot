using System;

namespace Arcbot.Essential.Models
{
    public class ProfileLevel : IProfileField
    {
        private long Exprience { get; set; }
        private int Level { get; set; }
        public void PlusPlus(int exp)
        {
            Exprience += exp;
            long next = (long)Math.Pow(Level + 4, 2);
            while (Exprience >= next)
            {
                Exprience -= next;
                Level++;

                next = (long)Math.Pow(Level + 4, 2);
            }
        }
    }
}