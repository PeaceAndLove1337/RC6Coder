using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RC6Coder
{
    static class SomeTools
    {
        public static bool IsNumber(String inputString)
        {
            foreach (char x in inputString)
                if (!char.IsDigit(x))
                    return false;
            return true;
        }

        public static String GenerateRandomKey(int length, bool isModeHard)
        {
            String resultRandomKey = "";
            Random random = new Random();

            if (isModeHard)
            {
                for (int i = 0; i < length; i++)
                {
                    resultRandomKey += (char)random.Next(33, 126);
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    resultRandomKey += (char)random.Next(65, 90);
                }
            }
            return resultRandomKey;
        }
    }
}
