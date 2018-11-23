using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Core;

namespace Day_24
{
	class Program
	{
		private static int rows = 0;
		private static int columns = 0;
		private static char[][] field;

		static void Main(string[] args)
		{
			var inputText = File.ReadAllLines(@"../../../input.txt");
			field = inputText.Select(x => x.ToCharArray()).ToArray();
			rows = field.Length;
			columns = field[0].Length;

			var allNumbers = field.SelectMany(x => x).Where(char.IsDigit).ToArray();
			var origin = new Coordinate(1, 1);

			var bfs = new BreadthFirstSearch<Coordinate, MoveDirection>(new CoordinateIdentical(), GetPossibleMovements, GetPosition);

			var result = bfs.FindAll(origin, coords => char.IsDigit(ContentAt(coords)));

			foreach (var path in result)
			{
				var item = ContentAt(path[0]);
				Console.WriteLine($"Item {item} found at {path[0]}");
			}

			Console.ReadLine();
		}

		private static char ContentAt(Coordinate pos) => field[pos.Y][pos.X];

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
			{
				yield break;
			}
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
			public int X { get; }
			public int Y { get; }

			public Coordinate(int x, int y)
			{
				X = x;
				Y = y;
			}

			public bool IsValid() => ContentAt(this) != '#';

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

	public enum MoveDirection { Up, Down, Left, Right };

}
