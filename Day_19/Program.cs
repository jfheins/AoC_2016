using System;

namespace Day_19
{
	class Program
	{
		static void Main()
		{
			var input = 5;

			var calculator = new ElfCircle(input);
			Console.WriteLine($"In a circle with {input} elves, the richest wil be at:");
			Console.WriteLine(calculator.CalculateBillGates());

			Console.ReadLine();
		}
	}

	class ElfCircle
	{
		private readonly int _size;
		// Index: Elf number minus 1, false if that elf still has presents
		private readonly bool[] _isEmpty;

		public ElfCircle(int size)
		{
			_size = size;
			_isEmpty = new bool[size];
		}

		public int CalculateBillGates()
		{
			if (_size % 2 == 0)
			{
				return 1;
			}

			var elfIndex = 0;
			while (true)
			{
				var robbedElf = GetLeftNeighborOf(elfIndex);
				if (robbedElf == null)
					return elfIndex + 1;
				_isEmpty[robbedElf.Value] = true;
				elfIndex++;
			}
		}

		private int? GetLeftNeighborOf(int elf)
		{
			for (int i = elf + 1; i != elf; i = (i+1) % _size)
			{
				if (!_isEmpty[i])
				{
					return i;
				}
			}

			return null;
		}
	}
}
