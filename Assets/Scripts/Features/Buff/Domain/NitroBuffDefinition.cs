public class NitroBuffDefinition : BuffDefinition
{
    public override BuffType Type => BuffType.Nitro;

    public float BoostPower { get; }
    public float MaxSpeed { get; }
    public NitroBuffDefinition(float boostPower, float maxSpeed, float duration) : base(duration)
    {
        BoostPower = boostPower;
        MaxSpeed = maxSpeed;
    }

    public override IBuff CreateBuff()
    {
        return new NitroBuff(this);
    }
}
