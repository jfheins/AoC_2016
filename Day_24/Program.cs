using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Day_24
{
	class Program
	{
		private static int rows = 0;
		private static int columns = 0;
		private static char[][] input;

		static void Main(string[] args)
		{
			var inputText = File.ReadAllLines(@"../../../demo.txt");
			input = inputText.Select(x => x.ToCharArray()).ToArray();
			rows = input.Length;
			columns = input[0].Length;

			var allNumbers = input.SelectMany(x => x).Where(char.IsDigit).ToArray();

			var bfs = new BreadthFirstSearch<Tuple<int, int>, MoveDirection>(EqualityComparer<(int, int)?>.Default, GetPossibleMovements, GetPosition);
		}

		private static (int, int)? GetPosition((int x, int y)? position, MoveDirection direction)
		{
			var newpos = position;
			switch (direction)
			{
				case MoveDirection.Up:
					newpos = (position.x, position.y - 1);
					break;
				case MoveDirection.Down:
					newpos = (position.x, position.y + 1);
					break;
				case MoveDirection.Left:
					newpos = (position.x - 1, position.y);
					break;
				case MoveDirection.Right:
					newpos = (position.x + 1, position.y);
					break;
			}

			if (input[newpos.x][newpos.y] == '#')
			{
				return null;
			}

			return newpos;
		}

		private static IEnumerable<MoveDirection> GetPossibleMovements((int x, int y)? position)
		{
			if (position == null)
			{
				yield break;
			}
			if (position.Value.x > 0)
				yield return MoveDirection.Left;

			if (position.Value.y > 0)
				yield return MoveDirection.Up;

			if (position.Value.x < columns - 1)
				yield return MoveDirection.Right;

			if (position.Value.y < rows - 1)
				yield return MoveDirection.Down;
		}
	}

	public enum MoveDirection { Up, Down, Left, Right };
}
