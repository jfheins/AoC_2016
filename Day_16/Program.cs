using System;
using System.Diagnostics;

namespace Day_16
{
    class Program
    {
        static void Main(string[] args)
        {
            var seed = "10011111011011001";
            var diskLength = 272;
            diskLength = 35651584;

            var sw = new Stopwatch();
            sw.Start();
            var diskContent = StringEnlarger.EnlargeToLength(seed, diskLength);
            var checksum = StringEnlarger.CalculateChecksum(diskContent);
            sw.Stop();

            Console.WriteLine($"Checksum for seed {seed} expanded to {diskLength} is {checksum}.");
            Console.WriteLine($"Calculation took {sw.ElapsedMilliseconds}ms.");

            Console.ReadLine();
        }
    }
}
