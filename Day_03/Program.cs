using System;
using System.IO;
using System.Linq;

namespace Day_03
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = new[] {"5 10 25"};
			input = File.ReadAllLines(@"../../../input.txt");

			var allShapes = input.Select(GetSidesOrdered);
			var triangles = allShapes.Where(x => x.a + x.b > x.c);

			Console.WriteLine(triangles.Count());
			Console.ReadLine();
		}

		private static (int a, int b, int c) GetSidesOrdered(string line)
		{
			var arr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(int.Parse)
				.OrderBy(x => x)
				.ToArray();
			return ValueTuple.Create(arr[0], arr[1], arr[2]);
		}
	}
}
