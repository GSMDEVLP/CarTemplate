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
        return cfg.Kind switch
        {
            WeaponKind.Homing  => new HomingRocket(cfg, _time, _targeting, _bus),
            WeaponKind.Mine    => new MineWeapon(cfg, _time, _damage, _bus),
            // WeaponKind.Oil     => new OilSlickWeapon(cfg, _time, _bus), 
            _                  => new StraightProjectileWeapon(cfg, _time, _bus),
        };
    }
}
