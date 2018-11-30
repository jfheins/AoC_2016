using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;

namespace Day_03
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = new[] {"101 301 501", "102 302 502", "103 303 503", "201 401 609", "202 402 605", "203 403 603" };
			input = File.ReadAllLines(@"../../../input.txt");

		    var blocks = input.Chunks(3).Select(x => x.ToArray());

			var allShapes = input.Select(GetSidesOrdered);
		     allShapes = blocks.SelectMany(GetSidesFromChunks);
            var triangles = allShapes.Where(x => x.a + x.b > x.c);

			Console.WriteLine(triangles.Count());
			Console.ReadLine();
		}

	    private static IEnumerable<(int a, int b, int c)> GetSidesFromChunks(string[] lines)
	    {
	        var field = lines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
	            .Select(int.Parse).ToArray()).ToArray();
	        yield return CreateTuple(field[0][0], field[1][0], field[2][0]);
	        yield return CreateTuple(field[0][1], field[1][1], field[2][1]);
	        yield return CreateTuple(field[0][2], field[1][2], field[2][2]);
        }

	    private static (int a, int b, int c) CreateTuple(params int[] sides)
	    {
	        var arr = sides.OrderBy(x => x).ToArray();
	        return ValueTuple.Create(arr[0], arr[1], arr[2]);
        }

        private static (int a, int b, int c) GetSidesOrdered(string line)
		{
			var arr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(int.Parse);
		    return CreateTuple(arr.ToArray());

		}
	}
}
