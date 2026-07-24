using System;
using UnityEngine;

public sealed class BuffViewModel : ViewModelBase
{
    public ObservableProperty<string> ValueText { get; } = new("");
    public ObservableProperty<bool> IsVisible { get; } = new(false);

    private readonly IPlayerBuffSource _source;
    private readonly BuffType _type;

    public BuffViewModel(IPlayerBuffSource source, BuffType type)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _type = type;

        _source.BuffChanged += OnBuffChanged;
        Refresh();
    }

    private void OnBuffChanged(BuffState state)
    {
        if (state.Type == _type)
            Apply(state);
    }

    private void Refresh()
    {
        if (_source.TryGetState(_type, out var state))
            Apply(state);
        else
            Apply(BuffState.Inactive(_type));
    }

    private void Apply(BuffState state)
    {
        IsVisible.Value = state.IsActive;
        ValueText.Value = state.IsActive
            ? Mathf.CeilToInt(state.Value).ToString()
            : string.Empty;
    }

    public override void Dispose()
    {
        _source.BuffChanged -= OnBuffChanged;
    }
}
