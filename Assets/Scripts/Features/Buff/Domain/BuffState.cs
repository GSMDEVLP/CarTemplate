public readonly struct BuffState
{
    public BuffType Type { get; }
    public bool IsActive { get; }
    public float Value { get; }

    public BuffState(BuffType type, bool isActive, float value)
    {
        Type = type;
        IsActive = isActive;
        Value = value;
    }

    public static BuffState Inactive(BuffType type)
    {
        return new BuffState(type, false, 0f);
    }
}
