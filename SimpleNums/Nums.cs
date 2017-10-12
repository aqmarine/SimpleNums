using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNums
{
    public static class Nums
    {
        public static int GetDigitNumber(int value)
        {
            int result = 0;
            do
            {
                value = value / 10;
                result++;
            } while (value >= 1);
            return result;
        }

        public static bool IsEvenNumber(int num)
        {
            return num % 2 == 0;
        }
    }
}
