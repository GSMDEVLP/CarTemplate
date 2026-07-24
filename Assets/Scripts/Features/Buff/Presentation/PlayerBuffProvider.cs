using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerBuffProvider : MonoBehaviour, IPlayerBuffSource
{
    public static PlayerBuffProvider Instance { get; private set; }

    [SerializeField] private MonoBehaviour[] _stateSourceComponents;

    private readonly Dictionary<BuffType, IBuffStateSource> _sources = new();

    public event Action<BuffState> BuffChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("More than one PlayerBuffProvider is active.", this);
            enabled = false;
            return;
        }

        Instance = this;
        BindSources();
    }

    public bool TryGetState(BuffType type, out BuffState state)
    {
        if (_sources.TryGetValue(type, out var source))
        {
            state = source.State;
            return true;
        }

        state = BuffState.Inactive(type);
        return false;
    }

    private void BindSources()
    {
        if (_stateSourceComponents == null)
            return;

        for (int i = 0; i < _stateSourceComponents.Length; i++)
        {
            var component = _stateSourceComponents[i];

            if (component == null)
                continue;

            if (!(component is IBuffStateSource source))
            {
                Debug.LogWarning(
                    $"{component.name} does not implement {nameof(IBuffStateSource)}.",
                    component);
                continue;
            }

            if (_sources.ContainsKey(source.Type))
            {
                Debug.LogWarning(
                    $"Duplicate buff state source for {source.Type}.",
                    component);
                continue;
            }

            _sources.Add(source.Type, source);
            source.StateChanged += OnSourceStateChanged;
        }
    }

    private void OnSourceStateChanged(BuffState state)
    {
        BuffChanged?.Invoke(state);
    }

    private void OnDestroy()
    {
        foreach (var source in _sources.Values)
            source.StateChanged -= OnSourceStateChanged;

        _sources.Clear();

        if (Instance == this)
            Instance = null;
    }
}
