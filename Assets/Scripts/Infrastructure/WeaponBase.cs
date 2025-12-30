
using UnityEngine;

public abstract class WeaponBase : IWeapon
{
    protected readonly WeaponConfig Cfg;

    protected readonly WeaponRuntime Rt;
    protected readonly ITime tm;
    protected float _nextFireTime;
    public bool CanFire => tm.TimeSinceStartup >= _nextFireTime;

    public float CooldownRemaining => Mathf.Max(0f, _nextFireTime - tm.TimeSinceStartup);
    public float CooldownDuration => Rt.Cooldown;

    public int CurrentAmmo => Rt.CurrentAmmo;
    public int MaxAmmo => Rt.MaxAmmo;


    protected WeaponBase(WeaponConfig cfg, WeaponRuntime rt, ITime time)
    {
        Cfg = cfg;
        Rt = rt;
        tm = time;
    }
    
    public void Fire(FireContext ctx)
    {
        if (!CanFire) return;
        if (Rt.CurrentAmmo <= 0) return;

        _nextFireTime = tm.TimeSinceStartup + Rt.Cooldown;
        Rt.CurrentAmmo--;
        
        OnFire(ctx);
    }

    protected abstract void OnFire(FireContext ctx);
}
