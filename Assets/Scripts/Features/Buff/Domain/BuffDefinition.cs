public abstract class BuffDefinition
{
    public abstract BuffType Type { get; }
    public float Duration { get; }

    protected BuffDefinition(float duration)
    {
        Duration = duration < 0f ? 0f : duration;
    }

    public abstract IBuff CreateBuff();
}