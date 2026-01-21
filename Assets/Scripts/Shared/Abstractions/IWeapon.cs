
public interface IWeapon 
{
    public bool CanFire { get; }
    float CooldownRemaining { get; }
    float CooldownDuration { get; }

    int CurrentAmmo { get; }
    int MaxAmmo { get; }
    public void Fire(FireContext ctx);
}
