
public abstract class WeaponBase : IWeapon
{
    protected readonly WeaponConfig Cfg;

    protected readonly WeaponRuntime Rt;
    protected readonly ITime tm;
    protected float _nextFireTime;
    public bool CanFire => tm.TimeSinceStartup >= _nextFireTime;


    protected WeaponBase(WeaponConfig cfg, WeaponRuntime rt, ITime time)
    {
        Cfg = cfg;
        Rt = rt;
        tm = time;
    }
    
    public void Fire(FireContext ctx)
    {
        if (!CanFire) return;
        _nextFireTime = tm.TimeSinceStartup + Rt.Cooldown;
        OnFire(ctx);
    }

    protected abstract void OnFire(FireContext ctx);
}
