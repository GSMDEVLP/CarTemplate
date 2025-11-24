using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : IWeapon
{
    protected readonly WeaponConfig Cfg;

    protected readonly WeaponRuntime Rt;
    protected readonly ITime Time;
    protected float Next;
    public bool CanFire => throw new System.NotImplementedException();

    protected WeaponBase(WeaponConfig cfg, WeaponRuntime rt, ITime time)
    {
        Cfg = cfg;
        Rt = rt;
        Time = time;
    }
    
    public void Fire(FireContext ctx)
    {
        if (!CanFire) return;
        Next = Time.TimeSinceStartup + Rt.Cooldown;
        OnFire(ctx);
    }

    protected abstract void OnFire(FireContext ctx);
}
