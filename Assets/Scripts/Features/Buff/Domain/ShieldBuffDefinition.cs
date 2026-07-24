public class ShieldBuffDefinition : BuffDefinition
{
    public override BuffType Type => BuffType.Shield;

    public float Endurance { get; }

    public ShieldBuffDefinition(float endurance, float duration) : base(duration)
    {
        Endurance = endurance;
    }
    public override IBuff CreateBuff()
    {
        return new ShieldBuff(this);
    }
}
