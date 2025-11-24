using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingModule", menuName = "Weapons/Modules/Homing")]
public class HomingModule : WeaponModule
{
    public float SeekRadius;
    public float HomingStrength;

    public override void Apply(WeaponRuntime target)
    {
        target.SeekRadius = SeekRadius;
        target.HomingStrength = HomingStrength;
    }
}
