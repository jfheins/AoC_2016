using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_20
{
	class Program
	{

		static void Main(string[] args)
		{
			var input = new[] {"5-8", "0-2", "4-7"};
			input = File.ReadAllLines(@"../../../input.txt");

			var blackListedRanges = input.Select(IpRange.FromString).ToList();

			uint candidate = 0;
			List<IpRange> blockingRanges;

			while(true)
			{
				blockingRanges = blackListedRanges.Where(r => r.CompareNumber(candidate) == CompareResult.InRange).ToList();

				if (!blockingRanges.Any())
					break;

				candidate = blockingRanges.Select(range => range.UpperBound).Max() + 1;
			}

			Console.WriteLine(candidate);
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

	public enum CompareResult { Below, InRange, Above }
}
