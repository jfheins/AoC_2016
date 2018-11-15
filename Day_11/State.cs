﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day_11
{
    internal class State : IEquatable<State>
	{
		/// <summary>
		/// By convention, the first item (index 0) is the elevator, every odd item is a generator and every even >0 a chip.
		/// The length of the array corresponds to the number of items, their value to the floor they're on.
		/// </summary>
		private readonly int[] _items;

		private static readonly string[] Desc = {"E ", "1G", "1M", "2G", "2M", "3G", "3M", "4G", "4M", "5G", "5M" };
		
        private static int[] _chipIndicies;
        private static int[] _generatorIndicies;

        private int[] ChipLevels => _chipIndicies.Select(idx => _items[idx]).ToArray();
        private int[] GeneratorLevels => _generatorIndicies.Select(idx => _items[idx]).ToArray();

        private State(IEnumerable<int> items)
        {
            _items = items.ToArray();
            if (_chipIndicies == null)
            {
                var nonElevatorIndicies = Enumerable.Range(1, _items.Length - 1).ToArray();
                _chipIndicies = nonElevatorIndicies.Where(x => x % 2 == 0).ToArray();
                _generatorIndicies = nonElevatorIndicies.Where(x => x % 2 == 1).ToArray();
            }
        }

	    private State(int[] items)
	    {
	        _items = items;
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
                for (int i = 0; i < _items.Length; i++)
                {
                    result += ((_items[i] == floor) ? Desc[i] : ". ")+" ";
                }
                result += "\r\n";
            }

			result += $"Score: {GetScore()} Points\r\n";
            return result;
        }

        public bool IsValid()
        {
            // A chip must not be on the same level as a non-matching generator
            foreach (var chip in ChipLevels.Select((lvl, idx) => new { lvl, idx }))
			{
				var isProtected = GeneratorLevels[chip.idx] == chip.lvl;
				if (isProtected)
					continue;

                if (GeneratorLevels.Where((l, i) => l == chip.lvl && i != chip.idx).Any())
                    return false;
            }
            // The elevator cannot run empty, so something must be on its level
            var elevatorLevel = _items[0];
            if (_items.Count(x => x == elevatorLevel) <= 1)
                return false;

            return true;
        }

        public bool IsFinalState()
        {
            return _items.All(x => x == 4);
        }

        public int GetScore()
		{
			int score = 0;
			for (int i = 1; i < _items.Length; i++)
			{
				score += _items[i];
			}
			return score;
        }

        public IEnumerable<State> GetPossibleSuccessorStates()
        {
            // The elevator can move up or down, and it can take 1 or 2 items with it.
            var possibleMovements = new List<int>();
            var elevatorLevel = _items[0];

            if (elevatorLevel < 4)
                possibleMovements.Add(1);
            if (elevatorLevel > 1)
                possibleMovements.Add(-1);

            // THe List of the indicies of all possible subsets of items
            var possibleTransitions = new List<Transition>();

            var movableItemIndicies = _items
                .IndexWhere(l => l == elevatorLevel)
                .Where(x => x > 0) // Not interested in the elevator
                .ToArray();

            // They can move alone
            possibleTransitions.AddRange(movableItemIndicies.SelectMany(i => GenerateTransitions(possibleMovements, i)));

            if (movableItemIndicies.Length > 1)
            {
                var combinations = GenerateCombinationPairs(movableItemIndicies);
                possibleTransitions.AddRange(combinations.SelectMany(pair => GenerateTransitions(possibleMovements, pair)));
            }

            return possibleTransitions.Select(Transform).Where(state => state.IsValid());
        }

        private static IEnumerable<Transition> GenerateTransitions(IEnumerable<int> movements, int ItemIndex)
        {
            return movements.Select(x => new Transition(x, new[] {ItemIndex}));
        }

        private static IEnumerable<Transition> GenerateTransitions(IEnumerable<int> movements, int[] ItemIndicies)
        {
            return movements.Select(x => new Transition(x, ItemIndicies));
        }

        public State Transform(Transition t)
        {
            var newState = _items.ToArray();
            newState[0] += t.Direction; // Move the elevator
            foreach (var idx in t.ItemIndicies)
            {
                newState[idx] += t.Direction;
            }
            return new State(newState);
        }

        private IEnumerable<int[]> GenerateCombinationPairs(int[] set)
        {
            for (int i = 0; i < set.Length - 1; i++)
            {
                for (int j = i+1; j < set.Length; j++)
                {
                    yield return new[] { set[i], set[j] };
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as State);
        }

        public bool Equals(State other)
        {
            return other != null && _items.SequenceEqual(other._items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_items);
        }
    }
}