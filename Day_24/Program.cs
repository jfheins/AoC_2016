using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using Core.Combinatorics;

namespace Day_24
{
	internal class Program
	{
		private static int rows;
		private static int columns;
		private static char[][] field;

		private static void Main(string[] args)
		{
			var inputText = File.ReadAllLines(@"../../../input.txt");
			field = inputText.Select(x => x.ToCharArray()).ToArray();
			rows = field.Length;
			columns = field[0].Length;

			var allNumbers = field.SelectMany(x => x).Where(char.IsDigit).ToArray();

			var numberPositions = new Dictionary<char, Coordinate>();

			for (var rowIndex = 0; rowIndex < rows; rowIndex++)
			for (var colIndex = 0; colIndex < columns; colIndex++)
			{
				var c = new Coordinate(colIndex, rowIndex);
				var content = ContentAt(c);
				if (char.IsDigit(content))
					numberPositions.Add(content, c);
			}

			foreach (var position in numberPositions)
			{
				Console.WriteLine($"Number {position.Key} found at {position.Value}");
			}

			var bfs = new BreadthFirstSearch<Coordinate, MoveDirection>(new CoordinateIdentical(), GetPossibleMovements,
				GetPosition);

			var distances = new Dictionary<string, int>();
			foreach (var number in numberPositions.Keys)
			{
				var others = bfs.FindAll(numberPositions[number], coords => ContentAt(coords) > number);
				foreach (var other in others)
				{
					var edge1 = string.Concat(number, ContentAt(other[0]));
					var edge2 = string.Concat(ContentAt(other[0]), number);
					var edgeLength = other.Length - 1;
					distances.Add(edge1, edgeLength);
					distances.Add(edge2, edgeLength);
					Console.WriteLine($"Edge {edge1} / {edge2} is {edgeLength} long.");
				}
			}

			var allTrips = new Permutations<char>(allNumbers);

			var allTripsWithLengths = new Dictionary<string, int>();

			foreach (IList<char> trip in allTrips)
			{
				if (trip[0] != '0')
					continue;
				trip.Add('0');
				var name = string.Concat(trip);
				var length = 0;
				foreach (var walk in trip.PairwiseWithOverlap())
				{
					var edge = string.Concat(walk.Item1, walk.Item2);
					length += distances[edge];
				}

				allTripsWithLengths.Add(name, length);
			}

			Console.WriteLine();

			var min = allTripsWithLengths.OrderBy(x => x.Value).Take(3);
			foreach (var trip in min)
				Console.WriteLine($"Trip {trip.Key} is {trip.Value} long");

			Console.ReadLine();
		}

		private static bool IsOtherNumber(Coordinate arg, char self)
		{
			var target = ContentAt(arg);
			return target != self && char.IsDigit(target);
		}

		private static char ContentAt(Coordinate pos)
		{
			return field[pos.Y][pos.X];
		}

		private static Coordinate GetPosition(Coordinate position, MoveDirection direction)
		{
			var newpos = position;
			switch (direction)
			{
				case MoveDirection.Up:
					newpos = new Coordinate(position.X, position.Y - 1);
					break;
				case MoveDirection.Down:
					newpos = new Coordinate(position.X, position.Y + 1);
					break;
				case MoveDirection.Left:
					newpos = new Coordinate(position.X - 1, position.Y);
					break;
				case MoveDirection.Right:
					newpos = new Coordinate(position.X + 1, position.Y);
					break;
			}

			return newpos.IsValid() ? newpos : null;
		}

		private static IEnumerable<MoveDirection> GetPossibleMovements(Coordinate position)
		{
			if (position == null)
				yield break;
			if (position.X > 0)
				yield return MoveDirection.Left;

			if (position.Y > 0)
				yield return MoveDirection.Up;

			if (position.X < columns - 1)
				yield return MoveDirection.Right;

			if (position.Y < rows - 1)
				yield return MoveDirection.Down;
		}

		public class Coordinate
		{
			public Coordinate(int x, int y)
			{
				X = x;
				Y = y;
			}

			public int X { get; }
			public int Y { get; }

			public bool IsValid()
			{
				return ContentAt(this) != '#';
			}

			public override string ToString()
			{
				return $"({X}|{Y})";
			}
		}

		internal class CoordinateIdentical : EqualityComparer<Coordinate>
		{
			public override bool Equals(Coordinate a, Coordinate b)
			{
				if (a == null && b == null)
					return true;
				if (a == null || b == null)
					return false;

				return a.X == b.X && a.Y == b.Y;
			}

			public override int GetHashCode(Coordinate point)
			{
				return HashCode.Combine(point.X, point.Y);
			}
		}
	}

	public enum MoveDirection
	{
		Up,
		Down,
		Left,
		Right
	}
}