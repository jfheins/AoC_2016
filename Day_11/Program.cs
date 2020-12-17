using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Core;

namespace Day_11
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
The first floor contains a strontium generator, a strontium-compatible microchip, a plutonium generator, and a plutonium-compatible microchip.
The second floor contains a thulium generator, a ruthenium generator, a ruthenium-compatible microchip, a curium generator, and a curium-compatible microchip.
The third floor contains a thulium-compatible microchip.
The fourth floor contains nothing relevant.
			 */
            var input = "1 2 1 3 1"; // Example
            input = "1 1 1 1 1 2 2 2 2 2 3";
            input = "1 1 1 1 1 2 2 2 2 2 3 1 1 1 1"; // Julius
            input = "1 1 1 1 1 2 3 2 2 2 2 1 1 1 1"; // Mariano
            var initialState = State.FromString(input);

            var sw = new Stopwatch();

            Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

            var comparer = new StateEquivalent();
            IPath<State, Transition> path = null;

            for (var i = 0; i < 10; i++)
            {
                sw.Start();
                var bfs = new BreadthFirstSearch<State, Transition>(comparer, s => s.GetPossibleSuccessorStates());
                path = bfs.FindFirst(initialState, n => n.IsFinalState());
                sw.Stop();

                var duration = sw.ElapsedMilliseconds / 1000f;
                Console.WriteLine($"It took {duration:0.000} seconds.");
            }

            Console.WriteLine("========================================");
            if (path != null)
                Console.WriteLine($"Final state reached after {path.Length} steps :-)");
            Console.ReadLine();
        }
    }

    internal class StateIdentical : EqualityComparer<State>
    {
        public override bool Equals(State a, State b)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;

            return a.Items.SequenceEqual(b.Items);
        }

        public override int GetHashCode(State x)
        {
            return HashCode.Combine(x.Items);
        }
    }

    internal class StateEquivalent : EqualityComparer<State>
    {
        private static readonly int[,] Primes = { { 2, 3, 5, 7 }, { 11, 13, 17, 19 }, { 23, 29, 31, 37 }, { 41, 43, 47, 53 } };

        public override bool Equals(State a, State b)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;

            if (a.Items[0] != b.Items[0])
                return false;

            return GetLong(a) == GetLong(b);
        }

        private long GetLong(State x)
        {
            long result = 1;
            var chips = x.ChipLevels;
            var generators = x.GeneratorLevels;
            for (var i = 0; i < chips.Length; i++)
                checked { result *= Primes[chips[i] - 1, generators[i] - 1]; }

            return result;
        }

        public override int GetHashCode(State x)
        {
            var hash = GetLong(x);
            return (int)(hash ^ (hash >> 32));
        }
    }
}