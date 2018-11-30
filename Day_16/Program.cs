using System;
using System.Diagnostics;
using System.Linq;

namespace Day_16
{
    class Program
    {
        static void Main(string[] args)
		{
			var seed = "10001001100000001";
			var diskLength = 272;
            diskLength = 35651584;

            var sw = new Stopwatch();
            //sw.Start();
            //var diskContent = StringEnlarger.EnlargeToLength(seed, diskLength);
            //var checksum = StringEnlarger.CalculateChecksum(diskContent);
            //sw.Stop();

            //Console.WriteLine($"Disk content for seed {seed} expanded to {diskLength} is:");
            //Console.WriteLine(diskContent);
            //Console.WriteLine($"Checksum for seed {seed} expanded to {diskLength} is {checksum}.");
            //Console.WriteLine($"Calculation took {sw.ElapsedMilliseconds}ms.");

            sw.Restart();
            var provider = new CharProvider(seed, diskLength);
            var expContent = Enumerable.Range(0, diskLength).Select(x => provider.CharAt(x));
            var checksum = StringEnlarger.CalculateChecksum(expContent, diskLength);
			sw.Stop();

			Console.WriteLine($"Checksum for seed {seed} expanded to {diskLength} is {checksum}.");
            Console.WriteLine($"Calculation took {sw.ElapsedMilliseconds}ms.");
            Console.ReadLine();
        }
    }
}
