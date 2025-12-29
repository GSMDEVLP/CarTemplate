using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingModule", menuName = "Weapons/Modules/Homing")]
public class HomingModule : WeaponModule
{
    [SerializeField] private float _seekRadius;
    [SerializeField] private float _homingStrength;
    
    public float SeekRadius => _seekRadius;
    public float HomingStrength => _homingStrength;

    public override void Apply(WeaponRuntime target)
    {
        target.SeekRadius = SeekRadius;
        target.HomingStrength = HomingStrength;
    }
}
