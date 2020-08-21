using System;
using System.Collections.Generic;
using System.Linq;

namespace Arcbot.GuessNumber
{
    public class GuessBox
    {
        public readonly int N;
        public int[] Numbers { get; set; }

        public GuessBox(int n)
        {
            N = n;
            Numbers = new int[N];
        }

        public GuessBox() : this(4) { }
        public void Start()
        {
            Random rand = new Random();
            List<int> nums = "0123456789".Select(x => int.Parse(x.ToString())).ToList();
            for (int i = 0; i < N; i++)
            {
                Numbers[i] = nums[rand.Next(nums.Count)];
                nums.RemoveAt(Numbers[i]);
            }
        }

        public (int, int) Calculate(int[] input)
        {
            int a = 0;
            int b = 0;
            for (int i = 0; i < Math.Min(input.Length, N); i++)
            {
                if (input[i] == Numbers[i]) a++;
                else if (Numbers.Contains(input[i])) b++;
            }
            return (a,b);
        }
    }
}