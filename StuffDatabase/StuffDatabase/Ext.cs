using System;
using System.Linq;

namespace StuffDatabase
{
    public static class Ext
    {
        static readonly Random random = new Random();
        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }


        public static string ToHumanReadable(this double number, int digits = 3)
        {
            string smallPrefix = "mµnpf";
            string largePrefix = "kMGT";
            bool negative = number < 0;
            if (negative)
                number = -number;
            int thousands = (int)Math.Log(Math.Abs(number), 1000);

            if (Math.Log(Math.Abs(number), 1000) < 0)
                thousands--;

            if (number == 0)
                thousands = 0;

            double scaledNumber = number * Math.Pow(1000, -thousands);

            int places = Math.Max(0, digits - (int)Math.Log10(scaledNumber));
            string s = scaledNumber.ToString("F" + places.ToString());



            if (thousands > 0)
                if (thousands < largePrefix.Length)
                    s += largePrefix[thousands - 1];

            if (thousands < 0)
                if (Math.Abs(thousands) < largePrefix.Length)
                    s += smallPrefix[Math.Abs(thousands) - 1];
            if (negative)
                s = $"-{s}";
            return s;
        }
    }



}
