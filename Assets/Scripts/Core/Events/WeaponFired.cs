using UnityEngine;

public readonly struct WeaponFired : IEvent
{
    public readonly GameObject Owner; 
    public readonly WeaponConfig WeaponCfg;
    public WeaponFired(GameObject owner, WeaponConfig weaponCfg) 
    {
        Owner = owner;
        WeaponCfg = weaponCfg; 
    }
}