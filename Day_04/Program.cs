using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LanguageExt;

namespace Day_04
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var input = @"aaaaa-bbb-z-y-x-123[abxyz]";
            var room = Room.FromLine(input);

            room.IfSome((r) => Console.WriteLine(r.IsReal()));

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

        public static Option<Room> FromLine(string line)
        {
            var pattern = new Regex(@"([\w-]+)-(\d+)\[(\w+)\]");
            var groups = pattern.Match(line).Groups;

            if (groups.Count != 4)
                return Option<Room>.None;

            if (!int.TryParse(groups[2].Value, out var sector))
                return Option<Room>.None;

            return new Room(groups[1].Value, sector, groups[3].Value);
        }

        public bool IsReal()
        {
            var commonLetters = new Lst<char>(NameLettersWithOccurence
                .Where(it => it.letter != '-')
                .OrderBy(it => it.letter)
                .ThenBy(it => it.count)
                .Take(5)
                .Map(it => it.letter));

            var rightChecksum = string.Concat(commonLetters);
            return Checksum == rightChecksum;
        }
    }
}
