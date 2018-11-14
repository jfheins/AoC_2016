using System;
using System.Collections.Generic;

namespace Day_11
{
    internal class Path :IComparable<Path>
    {
        public Path(State initial)
        {
            Previous = null;
            Current = initial;
            Length = 0;
            Cost = Current.DistanceFromFinalState() + Length;
        }

        public Path(Path old, State next)
        {
            Previous = old;
            Current = next;
            Length = old.Length + 1;
            Cost = Current.DistanceFromFinalState() + Length;
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

        public int Length { get; }

        public Path Previous { get; }
        public State Current { get; }

        public int Cost { get; }

        public int CompareTo(Path other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Cost.CompareTo(other.Cost);
        }
    }
}