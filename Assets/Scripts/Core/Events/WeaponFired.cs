using UnityEngine;

public readonly struct WeaponFired : IEvent
{
    public readonly GameObject Owner; 
    public readonly string WeaponId;
    public WeaponFired(GameObject owner, string weaponId) 
    {
        Owner = owner;
        WeaponId = weaponId; 
    }
}