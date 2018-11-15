using System;
using System.Linq;

namespace Day_16
{
    class Program
    {
        static void Main(string[] args)
        {
            var seed = "11111";

            Console.WriteLine(Enlarge(seed));
            Console.ReadLine();
        }


        public static string Enlarge(string a)
        {
            var b = string.Concat(a.Reverse().Select(c => (char)('1' - c)));
            return string.Concat(a, "0", b);
        }
    }
}
