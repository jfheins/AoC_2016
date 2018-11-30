using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_20
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var input = new[] {"5-8", "0-2", "4-7"};
			input = File.ReadAllLines(@"../../../input.txt");

			var blackListedRanges = input.Select(IpRange.FromString).ToList();

			var allowedIPs = new List<uint>();
			uint candidate = 0;

			while (true)
			{
				var blockingRanges = blackListedRanges.Where(r => r.CompareNumber(candidate) == CompareResult.InRange)
					.ToList();

				if (!blockingRanges.Any())
				{
					allowedIPs.Add(candidate);
					if (candidate == uint.MaxValue)
						break;
					candidate++;
				}
				else
				{
					var blockMax = blockingRanges.Select(range => range.UpperBound).Max();
					if (blockMax == uint.MaxValue)
						break;
					candidate = blockMax + 1;
				}
			}

			Console.WriteLine("First:" + allowedIPs.First());
			Console.WriteLine("Overall:" + allowedIPs.Count);
			Console.ReadLine();
		}
	}

	public class IpRange
	{
		public uint LowerBound { get; private set; }
		public uint UpperBound { get; private set; }

		public static IpRange FromString(string range)
		{
			var bounds = range.Split('-').Select(uint.Parse).ToArray();
			return new IpRange {LowerBound = bounds[0], UpperBound = bounds[1]};
		}

		public CompareResult CompareNumber(uint number)
		{
			if (number < LowerBound)
				return CompareResult.Below;
			return number > UpperBound ? CompareResult.Above : CompareResult.InRange;
		}
	}

	public enum CompareResult
	{
		Below,
		InRange,
		Above
	}
}