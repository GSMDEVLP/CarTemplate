using UnityEngine;

public struct ProjectileContext
{
    public WeaponRuntime Rt;
    public object Owner;
    public Transform Target;
    public IDamageService DamageService;
}
