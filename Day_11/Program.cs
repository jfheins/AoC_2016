using System;
using System.Collections.Generic;
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
            Console.WriteLine(new Path(initialState).Length);

            var paths = new PriorityQueue<Path>
            {
                new Path(initialState)
            };
            var visitedStates = new Dictionary<State, int>();

            Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

            var iterCount = 0;
            Path finalPath = null;
			while (paths.Any())
			{
			    var mostPromising = paths.Dequeue();
			    visitedStates.TryAdd(mostPromising.Current, mostPromising.Length);

                var nextStates = new HashSet<State>(mostPromising.Current.GetPossibleSuccessorStates());

			    if (nextStates.Any(s => s.IsFinalState()))
			    {
                    finalPath = new Path(mostPromising, nextStates.First(s => s.IsFinalState()));
			        break;
			    }

			    var nextPaths = nextStates.Select(newState => new Path(mostPromising, newState));

			    foreach (var path in nextPaths)
			    {
			        if (visitedStates.TryGetValue(path.Current, out var existingLength))
			        {
			            if (path.Length < existingLength)
			            {
			                visitedStates[path.Current] = path.Length;
			            }
                    }
			        else
			        {
                        paths.Enqueue(path);
			        }
			    }
                
			    iterCount++;

                if (iterCount % 10000 == 0)
                {
                    Console.WriteLine($"{paths.Count} possible states after {iterCount++} explore iterations.");
                    Console.WriteLine($"best path has {mostPromising.Current.GetScore()} points and {mostPromising.Current.DistanceFromFinalState()} distance remaining.");
                }
			}
			
			Console.WriteLine("Final Path:");
			foreach (var state in finalPath.GetHistory())
			{
				//Console.WriteLine(state);
			}
			Console.WriteLine(finalPath.Current);

			Console.WriteLine($"Final state reached after {finalPath.Length} steps :-)");

			Console.ReadLine();
        }

        private static int CompareRemaining(Path x, Path y)
        {
            return x.Cost.CompareTo(y.Cost);
        }
    }
}