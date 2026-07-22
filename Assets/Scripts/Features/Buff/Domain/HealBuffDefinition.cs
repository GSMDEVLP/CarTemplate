public sealed class HealBuffDefinition : BuffDefinition
{
    public override BuffType Type => BuffType.Health;
    public float HealAmount { get; }

    public HealBuffDefinition(float duration, float healAmount)
        : base(duration)
    {
        HealAmount = healAmount;
    }

    public override IBuff CreateBuff()
    {
        return new HealBuff(this);
    }
}