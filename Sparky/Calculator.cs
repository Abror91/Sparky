using System;
using System.Collections.Generic;

namespace Sparky
{
    public class Calculator
    {
        public List<int> NumbersRange = new List<int>();
        public int AddNumbers(int a, int b)
        {
            return a + b;
        }

        public double AddDoubleNumbers(double a, double b)
        {
            return a + b;
        }

        public bool IsOddNumber(int a)
        {
            return a % 2 != 0;
        }

        public List<int> GetOddNumbersRange(int min, int max)
        {
            NumbersRange.Clear();
            for(var i = min; i <= max; i++)
            {
                if (i % 2 != 0)
                    NumbersRange.Add(i);
            }
            return NumbersRange;
        }
    }
}
