using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Day_04
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Room
    {
        public string Name { get; }
        public int SectorId { get; }
        public string Checksum { get; }

        public Room(string name, int sectorId, string checksum)
        {
            Name = name;
            SectorId = sectorId;
            Checksum = checksum;
        }

        public bool IsReal()
        {

        }

        private Dictionary<char, int> NameLettersWithOccurence => Name.
    }
}
