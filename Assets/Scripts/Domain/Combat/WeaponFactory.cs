using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory : IWeaponFactory
{
    private readonly ITime _time;
    private readonly IDamageService _damage;
    private readonly ITargetingService _targeting;
    private readonly IEventBus _bus;

    public WeaponFactory(ITime time, IDamageService damage, ITargetingService targeting, IEventBus bus)
    {
        _time = time;
        _damage = damage;
        _targeting = targeting;
        _bus = bus;
    }
    
    public IWeapon Create(WeaponConfig cfg)
    {
        var runtime = new WeaponRuntime();

        foreach (var module in cfg.Modules)
            module.Apply(runtime);

        switch (cfg.Type)
        {
            case WeaponKind.Straight:
                return new StraightProjectileWeapon(cfg, runtime, _time, _bus);

            case WeaponKind.Homing:
                return new HomingRocket(cfg, runtime, _time, _targeting, _bus);

            case WeaponKind.Mine:
                return new MineWeapon(cfg, runtime, _time, _bus);

            case WeaponKind.MachineGun:
                return new MachineGunWeapon(cfg, runtime, _time, _bus);

            
        }

        throw new Exception("Unknown weapon type");
    }
}
