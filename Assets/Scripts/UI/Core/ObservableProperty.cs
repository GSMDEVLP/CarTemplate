using System;

public sealed class ObservableProperty<T>
{
    private T _value;

    public event Action<T> OnChanged;

    public ObservableProperty(T initialValue = default)
    {
        _value = initialValue;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (Equals(_value, value)) return;
            _value = value;
            OnChanged?.Invoke(_value);
        }
    }
}
