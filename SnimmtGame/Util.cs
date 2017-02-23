using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnimmtGame
{
    class Util
    {
        public static int BullValue(int number)
        {
            if (number == 55) return 7;
            if (number % 11 == 0) return 5;
            if (number % 10 == 0) return 3;
            if (number % 5 == 0) return 2;
            if (number <= 104 && number >= 1) return 1;
            throw new ArgumentException("6 Nimmt! numbers must be between 1 and 104.");

        }
    }
}
