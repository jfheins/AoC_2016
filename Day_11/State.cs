using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day_11
{
    internal class State : IEquatable<State>
    {
        private static readonly string[] Desc =
            {"E ", "1G", "1M", "2G", "2M", "3G", "3M", "4G", "4M", "5G", "5M", "6G", "6M", "7G", "7M"};

        private static int[] _chipIndicies;
        private static int[] _generatorIndicies;

        /// <summary>
        ///     By convention, the first item (index 0) is the elevator, every odd item is a generator and every even >0 a chip.
        ///     The length of the array corresponds to the number of items, their value to the floor they're on.
        /// </summary>
        public readonly int[] Items;

        private State(IEnumerable<int> items)
        {
            Items = items.ToArray();

            if (_chipIndicies == null)
            {
                var nonElevatorIndicies = Enumerable.Range(1, Items.Length - 1).ToArray();
                _chipIndicies = nonElevatorIndicies.Where(x => x % 2 == 0).ToArray();
                _generatorIndicies = nonElevatorIndicies.Where(x => x % 2 == 1).ToArray();
            }
        }

        private State(int[] items)
        {
            Items = items;
        }

        public int[] ChipLevels => _chipIndicies.Select(idx => Items[idx]).ToArray();
        public int[] GeneratorLevels => _generatorIndicies.Select(idx => Items[idx]).ToArray();

        public bool Equals(State other)
        {
            return other != null && Items.SequenceEqual(other.Items);
        }

        /// <summary>
        ///     Expects a string like "1 2 1 3 1"
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
            for (var floor = 4; floor > 0; floor--)
            {
                result += $"F{floor} ";
                for (var i = 0; i < Items.Length; i++)
                    result += (Items[i] == floor ? Desc[i] : ". ") + " ";
                result += "\r\n";
            }

            result += $"Score: {GetScore()} Points\r\n";
            return result;
        }

        public bool IsValid()
        {
            var generators = GeneratorLevels;
            // A chip must not be on the same level as a non-matching generator
            foreach (var chip in ChipLevels.Select((lvl, idx) => new {lvl, idx}))
            {
                var isProtected = generators[chip.idx] == chip.lvl;
                if (isProtected)
                    continue;

                if (generators.Where((l, i) => l == chip.lvl && i != chip.idx).Any())
                    return false;
            }

            // The elevator cannot run empty, so something must be on its level
            var elevatorLevel = Items[0];
            if (Items.Count(x => x == elevatorLevel) <= 1)
                return false;

            return true;
        }

        public bool IsFinalState()
        {
            return Items.All(x => x == 4);
        }

        public int GetScore()
        {
            var score = 0;
            for (var i = 1; i < Items.Length; i++)
                score += Items[i];
            return score;
        }

        public IEnumerable<State> GetPossibleSuccessorStates()
        {
            // The elevator can move up or down, and it can take 1 or 2 items with it.
            var possibleMovements = new List<int>();
            var elevatorLevel = Items[0];

            if (elevatorLevel < 4)
                possibleMovements.Add(1);
            if (elevatorLevel > 1)
                possibleMovements.Add(-1);

            // THe List of the indicies of all possible subsets of items
            var possibleTransitions = new List<Transition>();

            var movableItemIndicies = Items
                .IndexWhere(l => l == elevatorLevel)
                .Where(x => x > 0) // Not interested in the elevator
                .ToArray();

            // They can move alone
            possibleTransitions.AddRange(
                movableItemIndicies.SelectMany(i => GenerateTransitions(possibleMovements, i)));

            if (movableItemIndicies.Length > 1)
            {
                var combinations = GenerateCombinationPairs(movableItemIndicies);
                possibleTransitions.AddRange(combinations.SelectMany(pair =>
                    GenerateTransitions(possibleMovements, pair)));
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
            var newState = Items.ToArray();
            newState[0] += t.Direction; // Move the elevator
            foreach (var idx in t.ItemIndicies) newState[idx] += t.Direction;
            return new State(newState);
        }

        private IEnumerable<int[]> GenerateCombinationPairs(int[] set)
        {
            for (var i = 0; i < set.Length - 1; i++)
            for (var j = i + 1; j < set.Length; j++)
                yield return new[] {set[i], set[j]};
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as State);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Items);
        }
    }
}
