using System.Collections.Generic;

namespace Day_11
{
	internal class Path
	{
		public Path(State initial)
		{
			Predecessor = null;
			Current = initial;
			Length = 0;
		}

		public Path(Path old, State next)
		{
			Predecessor = old;
			Current = next;
			Length = old.Length + 1;
		}

		public int Length { get; }

		public Path Predecessor { get; }
		public State Current { get; }

		public IEnumerable<State> GetHistory()
		{
			var pointer = Predecessor;
			while (pointer != null)
			{
				yield return pointer.Current;
				pointer = pointer.Predecessor;
			}
		}
	}
}