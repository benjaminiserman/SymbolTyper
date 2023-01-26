namespace SymbolTyper;

using System.Collections.Generic;
using Iksokodo;
using WingSharpExtensions;

public class DeterministicFiniteStateMachine<T>
{
	private class State
	{
		public Dictionary<T, State> Transitions { get; init; } = new();
		public bool Accepting { get; init; } = false;
		public required List<T> Key { get; init; }
		
		public bool TryAccept(IList<T> value, int i)
		{
			if (i >= value.Count)
			{
				return Accepting;
			}
			else
			{
				var t = value[i];
				return Transitions[t].TryAccept(value, i + 1);
			}
		}
	}

	private readonly List<State> _states = new();
	private State _startState;

	public bool TryAccept(IList<T> value) => _startState.TryAccept(value, 0);

	public static DeterministicFiniteStateMachine<char> GetDfsmFromStrings(IEnumerable<string> values) => DeterministicFiniteStateMachine<char>.GetDfsmFromSequence(values
		.Select(s => s
			.Select(c => c)
			.ToList()));
	public static DeterministicFiniteStateMachine<T> GetDfsmFromSequence(IEnumerable<IList<T>> values)
	{
		var dfsm = new DeterministicFiniteStateMachine<T>();

		State GetStates(IEnumerable<IList<T>> values, int i)
		{
			var tempDict = new LazyDictionary<T, List<IList<T>>>()
			{
				AddMissingKeys = true,
				GetDefault = () => new()
			};

			var shouldAccept = false;
			foreach (var value in values)
			{
				if (i < value.Count)
				{
					tempDict[value[i]].Add(value);
				}
				else
				{
					shouldAccept = true;
				}
			}

			var state = new State()
			{
				Accepting = shouldAccept,
				Key = values
					.First()
					.Take(i)
					.ToList()
			};

			foreach (var (key, list) in tempDict)
			{
				if (list.Count == 1)
				{
					state.Transitions.Add(key, new()
					{
						Accepting = true,
						Key = list
							.First()
							.ToList()
					});
				}
				else
				{
					state.Transitions.Add(key, GetStates(list, i + 1));
				}
			}

			dfsm._states.Add(state);
			return state;
		}

		dfsm._startState = GetStates(values, 0);
		return dfsm;
	}
}
