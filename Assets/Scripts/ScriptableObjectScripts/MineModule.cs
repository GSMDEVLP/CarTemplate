using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MineModule", menuName = "Weapons/Modules/Mine")]
public class MineModule : WeaponModule
{
    [SerializeField] private float _radius;
    [SerializeField] private float _armingDelay;

    
    public float Radius => _radius;
    public float ArmingDelay => _armingDelay;

    public override void Apply(WeaponRuntime target)
    {
        target.ExplosionRadius = Radius;
        target.ArmingDelay = ArmingDelay;
    }
}
