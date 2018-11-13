using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day_11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "1 2 1 3 1"; // Example

            var initialState = State.FromString(input);
            Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

            Console.ReadLine();
        }
    }

    class State
    {
        /// <summary>
        /// By convention, the first item (index 0) is the elevator, every odd item is a generator and every even >0 a chip.
        /// The length of the array corresponds to the number of items, their value to the floor they're on.
        /// </summary>
        public int[] Items { get; private set; }

        public string[] Desc = {"E ", "1G", "1M", "2G", "2M", "3G", "3M", "4G", "4M" };

        public IEnumerable<String> FirstFloor => Items.Where(x => x == 1).Select(x => Desc[x]);

        private readonly int[] _chipIndicies;
        private readonly int[] _generatorIndicies;

        private int[] chipLevels => _chipIndicies.Select(idx => Items[idx]).ToArray();
        private int[] generatorLevels => _generatorIndicies.Select(idx => Items[idx]).ToArray();

        public State(IEnumerable<int> items)
        {
            Items = items.ToArray();
            var nonElevatorIndicies = Enumerable.Range(1, Items.Length - 1).ToArray();
            _chipIndicies = nonElevatorIndicies.Where(x => x % 2 == 0).ToArray();
            _generatorIndicies = nonElevatorIndicies.Where(x => x % 2 == 1).ToArray();
        }

        /// <summary>
        /// Expects a string like "1 2 1 3 1"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static State FromString(string input)
        {
            var stuff = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            // Chips and Generators should match
            Debug.Assert(stuff.Length % 2 == 1);
            // Elevator starts always on first floor
            Debug.Assert(stuff[0] == "1");

            return new State(stuff.Select(int.Parse));
        }

        public override string ToString()
        {
            var result = "";
            for (int floor = 4; floor > 0; floor--)
            {
                result += $"F{floor} ";
                for (int i = 0; i < Items.Length; i++)
                {
                    result += ((Items[i] == floor) ? Desc[i] : ". ")+" ";
                }
                result += "\r\n";
            }

            return result;
        }

        public bool IsValid()
        {
            // A chip must not be on the same level as a not-matching generator
            return true;
        }
    }

    public interface IItem
    {
        string ToString();
    }

    public class Generator : IItem
    {
        public string Element { get; set; }

        public override string ToString()
        {
            return $"{Element} generator";
        }
    }

    public class Microchip
    {
        public string Element { get; set; }

        public override string ToString()
        {
            return $"{Element} chip";
        }
    }
}