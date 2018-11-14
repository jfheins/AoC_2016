using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Day_11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "1 2 1 3 1"; // Example
			input = "1 1 1 2 3 2 3 2 3 2 3";
            var initialState = State.FromString(input);
            Console.WriteLine(new Path(initialState).Length);

            var paths = new List<Path>
            {
                new Path(initialState)
            };

            Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

            var iterCount = 0;
            Path finalPath = null;
			while (paths.Any())
			{
			    var mostPromising = paths[0];
			    paths.RemoveAt(0);

			    var history = mostPromising.GetHistory().ToList();
                var nextStates = mostPromising.Current.GetPossibleSuccessorStates()
                        .Where(newState => !history.Contains(newState)).ToList();

			    if (nextStates.Any(s => s.IsFinalState()))
			    {
                    finalPath = new Path(mostPromising, nextStates.First(s => s.IsFinalState()));
			        break;
			    }

			    var nextPaths = nextStates.Select(newState => new Path(mostPromising, newState));
                paths.AddRange(nextPaths);
                paths.Sort(CompareRemaining);

			    iterCount++;

                if (iterCount % 1000 == 0)
			    {
				    Console.WriteLine($"{paths.Count} possible states after {iterCount++} explore iterations.");
			    }
			}
			
			Console.WriteLine("Final Path:");
			foreach (var state in finalPath.GetHistory())
			{
				//Console.WriteLine(state);
			}
			Console.WriteLine(finalPath.Current);

			Console.WriteLine($"Final state reached after {iterCount} steps :-)");

			Console.ReadLine();
        }

        private static int CompareRemaining(Path x, Path y)
        {
            return x.Current.DistanceFromFinalState().CompareTo(y.Current.DistanceFromFinalState());
        }
    }

	internal class Path
	{
		public Path(State initial)
		{
			Previous = null;
			Current = initial;
		}

		public Path(Path old, State next)
		{
		    Previous = old;
			Current = next;
		}

	    public IEnumerable<State> GetHistory()
	    {
	        var pointer = Previous;
	        while (pointer != null)
	        {
	            yield return pointer.Current;
	            pointer = pointer.Previous;
	        }
	    }

	    public int Length => GetHistory().Count();

		public Path Previous { get; }
		public State Current { get; }
	} 
}