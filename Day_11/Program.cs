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

			var paths = new List<Path>
			{
				new Path(initialState)
			};

			Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

			int stepcounter = 0;
			while (!paths.Any(p => p.Current.IsFinalState()))
			{
				paths = paths.AsParallel().SelectMany(p =>
					p.Current.GetPossibleSuccessorStates()
						.Where(newState => !p.Previous.Contains(newState))
						.Select(newState => new Path(p, newState))).ToList();

				Console.WriteLine($"{paths.Count} possible states after {stepcounter++} steps.");
			}
			
			Console.WriteLine("Final Path:");
			var finalPath = paths.First(p => p.Current.IsFinalState());
			foreach (var state in finalPath.Previous)
			{
				//Console.WriteLine(state);
			}
			Console.WriteLine(finalPath.Current);

			Console.WriteLine($"Final state reached after {stepcounter} steps :-)");

			Console.ReadLine();
        }

        private static void FindFinal(Stack<State> history, State current)
        {
            history.Push(current);
            var futureStates = current.GetPossibleSuccessorStates();
            foreach (var futureState in futureStates)
            {
                // Explore further if this is really a new State
                if (!history.Contains(futureState))
                {
                    FindFinal(history, futureState);
                }
            }
            history.Pop();
        }
    }

	class Path
	{
		public Path(State initial)
		{
			Previous = new List<State>();
			Current = initial;
		}

		public Path(Path old, State next)
		{
			Previous = old.Previous.ToList();
			Previous.Add(old.Current);
			Current = next;
		}
		public List<State> Previous { get; }
		public State Current { get; }
	} 
}