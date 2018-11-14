using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Day_11
{
    class Program
    {
        private static int currentBest = 1000000;

        static void Main(string[] args)
        {
            var input = "1 2 1 3 1"; // Example
            var initialState = State.FromString(input);

			var paths = new List<Path>
			{
				new Path(initialState)
			};

			Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

			for (int i = 1; i < 11; i++)
			{

				paths = paths.SelectMany(p =>
					p.Current.GetPossibleSuccessorStates()
						.Where(newState => !p.Previous.Contains(newState))
						.Select(newState => new Path(p, newState))).ToList();

				Console.WriteLine($"{paths.Count} possible states after {i} steps:");
				foreach (var path in paths)
				{
					Console.WriteLine(path.Current);
				}
				Console.WriteLine("=====================================");
			}

            Console.ReadLine();
        }

        private static void FindFinal(Stack<State> history, State current)
        {
            if (current.IsFinalState())
                currentBest = Math.Min(currentBest, history.Count);

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