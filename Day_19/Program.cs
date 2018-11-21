using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day_19
{
	internal class Program
	{
		private static void Main()
		{
			var input = 5;
			//input = 30143;
			input = 3014387;

			var calculator = new ElfCircle(input);
			Console.WriteLine($"In a circle with {input} elves, the richest wil be at:");
			var sw = new Stopwatch();
			sw.Start();
			Console.WriteLine(calculator.CalculateBillGates());
			sw.Stop();

			var duration = sw.ElapsedMilliseconds / 1000f;
			Console.WriteLine($"It took {duration:0.000} seconds.");
			// Debug: 3 sec Release: 1,15s

			Console.ReadLine();
		}
	}

	internal class ElfCircle
	{
		private readonly int _initialSize;
		private readonly LinkedList<Elf> _circle;

		public ElfCircle(int size)
		{
			_initialSize = size;

			var elfes = Enumerable.Range(1, size).Select(number => new Elf(number));
			_circle = new LinkedList<Elf>(elfes);
		}

		public int CalculateBillGates()
		{
			if (_initialSize % 2 == 0)
				return 1;

			var currentElf = _circle.First;
			while (_circle.Count > 1)
			{
				var robbedElf = GetLeftNeighborOf(currentElf);

				if (robbedElf == null)
					break; // No elf left to rob

				_circle.Remove(robbedElf);
				currentElf = GetLeftNeighborOf(currentElf);

				if (_circle.Count % 10000 == 0)
					Console.WriteLine(_circle.Count);
			}

			return _circle.First.Value.Number;
		}

		private LinkedListNode<Elf> GetLeftNeighborOf(LinkedListNode<Elf> elf)
		{
			return elf.Next ?? _circle.First;
		}

		private struct Elf
		{
			/// <summary>
			///     Number in the initial circle, starts with 1
			/// </summary>
			public readonly int Number;

			public Elf(int number)
			{
				Number = number;
			}
		}
	}
}