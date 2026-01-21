using UnityEngine;

public struct ProjectileContext
{
    public WeaponRuntime Rt;
    public EntityId  Owner;
    public Transform Target;
    public IDamageService DamageService;
}
