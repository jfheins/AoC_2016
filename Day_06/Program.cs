using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core;

namespace Day_06
{
	internal class Program
	{
		private static char MostCommon(string[] arr, int idx)
		{
			return arr.Histogram(it => it[idx]).MaxBy(t => t.count).item;
		}
		private static char LeastCommon(string[] arr, int idx)
		{
			return arr.Histogram(it => it[idx]).MinBy(t => t.count).item;
		}
		
		private static void Main(string[] args)
		{
			var input = File.ReadAllLines(@"../../../input.txt");

			for (int i = 0; i < input[0].Length; i++)
			{
				Console.Write(MostCommon(input, i));
			}

			Console.WriteLine();
			
			for (int i = 0; i < input[0].Length; i++)
			{
				Console.Write(LeastCommon(input, i));
			}
		}
	}
}