using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LanguageExt;

namespace Day_04
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var input = new[]
			{
				"aaaaa-bbb-z-y-x-123[abxyz]", "a-b-c-d-e-f-g-h-987[abcde]", "not-a-real-room-404[oarel]",
				"totally -real-room-200[decoy]"
			};
			input = File.ReadAllLines(@"../../../input.txt");

			var rooms = input.Map(Room.Parse);

			var realRooms = rooms.Somes()
				.Filter(r => r.IsReal())
				.ToSeq();

			foreach (var room in realRooms)
				Console.WriteLine(room.SectorId + ": " + room.DecryptedName);

			Console.WriteLine("Sector sum: " + realRooms.Sum(r => r.SectorId));
			Console.ReadLine();
		}
	}

	internal class Room
	{
		public Room(string name, int sectorId, string checksum)
		{
			Name = name;
			SectorId = sectorId;
			Checksum = checksum;
		}

		public string Name { get; }
		public int SectorId { get; }
		public string Checksum { get; }

		private IEnumerable<(char letter, int count)> NameLettersWithOccurence =>
			Name.GroupBy(x => x).Map(group => (letter: group.Key, count: group.Count()));

		public string DecryptedName => string.Concat(Name.Map(c => ShiftLetter(c, SectorId)));

		private char ShiftLetter(char letter, int count)
		{
			if (letter == '-')
				return ' ';

			var position = letter - 'a';
			return (char) ('a' + (position + count) % 26);
		}

		public static Option<Room> Parse(string line)
		{
			var pattern = new Regex(@"([\w\-]+)-(\d+)\[(\w+)\]");
			var groups = pattern.Match(line).Groups;

			if (groups.Count != 4)
				return Option<Room>.None;

			if (!int.TryParse(groups[2].Value, out var sector))
				return Option<Room>.None;

			return new Room(groups[1].Value, sector, groups[3].Value);
		}

		public bool IsReal()
		{
			var lettersWithCount = NameLettersWithOccurence
				.Where(it => it.letter != '-')
				.OrderByDescending(it => it.count)
				.ThenBy(it => it.letter);

			var commonLetters = new Lst<char>(lettersWithCount
				.Take(5)
				.Map(it => it.letter));

			var rightChecksum = string.Concat(commonLetters);
			return Checksum == rightChecksum;
		}
	}
}