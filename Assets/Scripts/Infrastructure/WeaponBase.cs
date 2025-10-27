using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : IWeapon
{
    protected readonly WeaponConfig Cfg;
    protected readonly ITime Time;
    protected float Next;
    public bool CanFire => throw new System.NotImplementedException();

    protected WeaponBase(WeaponConfig cfg, ITime time)
    {
        Cfg = cfg;
        Time = time;
    }
    
    public void Fire(FireContext ctx)
    {
        if (!CanFire) return;
        Next = Time.TimeSinceStartup + Cfg.Cooldown;
        OnFire(ctx);
    }

    protected abstract void OnFire(FireContext ctx);
}
