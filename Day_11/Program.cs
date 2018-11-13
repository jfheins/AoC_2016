using System;
using System.Collections.Generic;
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

            var states = new List<State>();
            states.Add(initialState);

            Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

            var historyStack = new Stack<State>();
            FindFinal(historyStack, initialState);

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
}