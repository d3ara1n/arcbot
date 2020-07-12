using System.Collections.Generic;

namespace Ac682.Hyperai.Plugins.Essential.Services
{
    public class EchoService
    {
        Dictionary<long, bool> groups = new Dictionary<long, bool>();
        public void On(long num)
        {
            if (groups.ContainsKey(num))
            {
                groups[num] = true;
            }
            else
            {
                groups.Add(num, true);
            }
        }

        public void Off(long num)
        {
            if(groups.ContainsKey(num))
            {
                groups.Remove(num);
            }
        }

        public bool IsOn(long num) =>
            groups.ContainsKey(num) && groups[num];
    }
}
