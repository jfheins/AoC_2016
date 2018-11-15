using System;

namespace Day_16
{
    class Program
    {
        static void Main(string[] args)
        {
            var seed = "11111";

            Console.WriteLine(StringEnlarger.Enlarge(seed));

            var content = "110010110100";
            var checksum = StringEnlarger.CalculateChecksum(content);
            Console.WriteLine($"Checksum for {content} is {checksum}.");

            Console.ReadLine();
        }
    }
}
