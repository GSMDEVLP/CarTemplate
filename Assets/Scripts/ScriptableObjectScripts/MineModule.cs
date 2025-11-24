using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MineModule", menuName = "Weapons/Modules/Mine")]
public class MineModule : WeaponModule
{
    public float Radius;
    public float ArmingDelay;

    public override void Apply(WeaponRuntime target)
    {
        target.ExplosionRadius = Radius;
        target.ArmingDelay = ArmingDelay;
    }
}
