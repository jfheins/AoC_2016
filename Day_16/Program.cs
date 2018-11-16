using System;

namespace Day_16
{
    class Program
    {
        static void Main(string[] args)
        {
            var seed = "10011111011011001";
            var diskLength = 272;

            var diskContent = StringEnlarger.EnlargeToLength(seed, diskLength);
            var checksum = StringEnlarger.CalculateChecksum(diskContent);
            Console.WriteLine($"Checksum for seed {seed} expanded to {diskLength} is {checksum}.");

            Console.ReadLine();
        }
    }
}
