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

            var visitedStates = new Dictionary<State, int>();

            Console.WriteLine("Initial State:");
            Console.WriteLine(initialState);

            Path finalPath = null;
			




			Console.WriteLine($"Final state reached after {finalPath.Length} steps :-)");

			Console.ReadLine();
        }
    }
}