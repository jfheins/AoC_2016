using System;

namespace Day_19
{
	internal class Program
	{
		private static void Main()
		{
			var input = 5;
			input = 3014387;

			var calculator = new ElfCircle(input);
			Console.WriteLine($"In a circle with {input} elves, the richest wil be at:");
			Console.WriteLine(calculator.CalculateBillGates());

			Console.ReadLine();
		}
	}

	internal class ElfCircle
	{
		// Index: Elf number minus 1, false if that elf still has presents
		private readonly bool[] _isEmpty;
		private readonly int _size;


		public ElfCircle(int size)
		{
			_size = size;
			_isEmpty = new bool[size];
		}

		public int CalculateBillGates()
		{
			if (_size % 2 == 0)
				return 1;

			var elfIndex = 0;
			int? nextElf = 0;
			while (nextElf.HasValue)
			{
				elfIndex = nextElf.Value;
				var robbedElf = GetLeftNeighborOf(elfIndex);
				if (robbedElf == null)
					break;
				_isEmpty[robbedElf.Value] = true;
				nextElf = GetLeftNeighborOf(elfIndex);
			}

			return elfIndex + 1;
		}

		private int? GetLeftNeighborOf(int elf)
		{
			for (var i = 1; i < _size; i++)
			{
				var index = (elf + i) % _size;
				if (!_isEmpty[index])
					return index;
			}

			return null;
		}
	}
}