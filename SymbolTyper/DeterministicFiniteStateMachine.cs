namespace SymbolTyper;

using System;
using System.Collections.Generic;
using System.Linq;
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
				if (Transitions.TryGetValue(t, out var state))
				{
					return state.TryAccept(value, i + 1);
				}
				else
				{
					return false;
				}
			}
		}

		public bool Consume(T symbol, State startState, ref State currentState)
		{
			if (Transitions.TryGetValue(symbol, out var state))
			{
				currentState = state;
			}
			else
			{
				currentState = startState;
			}

			return currentState.Accepting;
		}
	}

	private readonly List<State> _states = new();
	private State _startState;
	private State _currentState;

	public bool StartStateIsAccepting => _startState.Accepting;
	public bool CurrentStateIsAccepting => _currentState.Accepting;

	public string LogString() => string.Join("\n", _states.Select(s => $"{s.Key.GetHashCode()} | {string.Join(", ", s.Key)}: {string.Join(", ", s.Transitions.Values.Select(s => s.Key.GetHashCode()))}"));

	public bool Process(T symbol, out List<T> key)
	{
		_currentState.Consume(symbol, _startState, ref _currentState);
		key = _currentState.Key;

		//Console.WriteLine($"{symbol} => {{ {string.Join(", ", key)} }}, {key.GetHashCode()}");

		return _currentState.Accepting;
	}

	public IEnumerable<T> NextStates()
	{
		foreach (var transition in _currentState.Transitions.Keys)
		{
			yield return transition;
		}
	}

	public bool ReturnToStart()
	{
		_currentState = _startState;
		return _currentState.Accepting;
	}

	public bool TryAccept(IList<T> value) => _startState.TryAccept(value, 0);

	public static DeterministicFiniteStateMachine<char> GetDfsmFromStrings(IEnumerable<string> values) => DeterministicFiniteStateMachine<char>
		.GetDfsmFromSequence(values
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
				GetDefault = _ => new()
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
				state.Transitions.Add(key, GetStates(list, i + 1));
			}

			//Console.WriteLine($"{i}: {string.Join(", ", state.Key)}");
			dfsm._states.Add(state);
			return state;
		}

		dfsm._startState = GetStates(values, 0);
		dfsm._currentState = dfsm._startState;
		return dfsm;
	}
}
