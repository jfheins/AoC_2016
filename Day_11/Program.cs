using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Medallion.Collections;

namespace Day_11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "1 2 1 3 1"; // Example
            input = "1 1 1 2 3 2 3 2 3";
            input = "1 1 1 2 3 2 3 2 3 2 3";
            var initialState = State.FromString(input);

            var sw = new Stopwatch();

            Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

            sw.Start();

            var comparer = new StateIdentical();

            var visitedStates = new HashSet<State>(comparer);
            var nextStates = new HashSet<State>(comparer) { initialState };

            var stepCounter = 0;
            while (nextStates.Any())
            {
                stepCounter++;
                Console.Write($"Step {stepCounter}, expanding {nextStates.Count} nodes ... ");

                var expanded = nextStates.AsParallel().SelectMany(s => s.GetPossibleSuccessorStates());
                nextStates = new HashSet<State>(expanded, comparer);

                Console.WriteLine($"into {nextStates.Count} unique new nodes.");

                if (nextStates.AsParallel().Any(s => s.IsFinalState()))
                    break;
                
                visitedStates.UnionWith(nextStates);

            }
            sw.Stop();
            var duration = sw.ElapsedMilliseconds / 1000f;

            Console.WriteLine("========================================");
			Console.WriteLine($"Final state reached after {stepCounter} steps :-)");
            Console.WriteLine($"It took {duration:0.000} seconds.");
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
}