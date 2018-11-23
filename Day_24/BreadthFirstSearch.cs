﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_24
{
	public class BreadthFirstSearch<TNode, TEdge>
	{
		private readonly IEqualityComparer<NodeWithPredecessor> _comparer;
		private readonly Func<TNode, IEnumerable<TNode>> _expander;

		/// <summary>
		///     Prepares a breadth first search.
		/// </summary>
		/// <param name="comparer">Comparison function that determines node equality</param>
		/// <param name="expander">Callback to get the possible edges</param>
		/// <param name="combiner">Callback to combine a source node and an edge to a (possibly new) node. May return null</param>
		public BreadthFirstSearch(IEqualityComparer<TNode> comparer,
			Func<TNode, IEnumerable<TEdge>> expander,
			Func<TNode, TEdge, TNode> combiner)
		{
			_comparer = new NodeComparer(comparer);
			_expander = node => expander(node).Select(edge => combiner(node, edge)).Where(x => x != null);
		}

		/// <summary>
		///     Prepares a breadth first search.
		/// </summary>
		/// <param name="comparer">Comparison function that determines node equality</param>
		/// <param name="expander">Callback to get the possible nodes from a source node</param>
		public BreadthFirstSearch(IEqualityComparer<TNode> comparer, Func<TNode, IEnumerable<TNode>> expander)
		{
			_comparer = new NodeComparer(comparer);
			_expander = expander;
		}

		public TNode[] Search(TNode initialNode,
			Func<TNode, bool> targetPredicate,
			Action<int, int> progressReporter = null)
		{
			var visitedNodes = new HashSet<NodeWithPredecessor>(_comparer);
			var nextNodes = new HashSet<NodeWithPredecessor>(_comparer) {new NodeWithPredecessor(initialNode)};

			while (nextNodes.Count > 0)
			{
				progressReporter?.Invoke(visitedNodes.Count, nextNodes.Count);

				visitedNodes.UnionWith(nextNodes);

				var expanded = nextNodes.AsParallel()
					.SelectMany(sourceNode => _expander(sourceNode.Current)
						.Select(dest => new NodeWithPredecessor(dest, sourceNode))
						.Where(dest => !visitedNodes.Contains(dest)));

				nextNodes = new HashSet<NodeWithPredecessor>(expanded, _comparer);

				foreach (var node in nextNodes)
					if (targetPredicate(node.Current))
						return node.GetHistory().ToArray();
			}

			return null;
		}

		private class NodeComparer : EqualityComparer<NodeWithPredecessor>
		{
			private readonly IEqualityComparer<TNode> _comparer;

			public NodeComparer(IEqualityComparer<TNode> comparer)
			{
				_comparer = comparer;
			}

			public override bool Equals(NodeWithPredecessor a, NodeWithPredecessor b)
			{
				return _comparer.Equals(a.Current, b.Current);
			}

			public override int GetHashCode(NodeWithPredecessor x)
			{
				return _comparer.GetHashCode(x.Current);
			}
		}

		private class NodeWithPredecessor
		{
			public NodeWithPredecessor(TNode current, NodeWithPredecessor predecessor = null)
			{
				Predecessor = predecessor;
				Current = current;
			}

			public TNode Current { get; }
			private NodeWithPredecessor Predecessor { get; }

			public IEnumerable<TNode> GetHistory()
			{
				var pointer = this;
				do
				{
					yield return pointer.Current;
					pointer = pointer.Predecessor;
				} while (pointer != null);
			}
		}
	}
}