using System;
using System.Collections.Generic;

public sealed class EnemyStateMachine
{
    private readonly Dictionary<EnemyStateId, IEnemyState> _states = new();

    private IEnemyState _current;

    public EnemyStateId? CurrentId { get; private set; }

    public void AddState(EnemyStateId id, IEnemyState state)
    {
        if (state == null)
            throw new ArgumentNullException(nameof(state));

        _states[id] = state;
    }

    public void ChangeState(EnemyStateId next)
    {
        if (_current != null && CurrentId == next)
            return;

        if (!_states.TryGetValue(next, out var nextState))
            throw new InvalidOperationException(
                $"Enemy state '{next}' is not registered.");

        _current?.Exit();

        _current = nextState;
        CurrentId = next;

        _current.Enter();
    }

    public void Tick(float deltaTime)
    {
        if (_current == null)
            return;

        EnemyStateId? next = _current.Tick(deltaTime);

        if (next.HasValue)
            ChangeState(next.Value);
    }
}