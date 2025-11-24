using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsModule", menuName = "Weapons/Modules/Stats")]
public class StatsModule : WeaponModule
{
    public float Damage;
    public float Cooldown;

    public override void Apply(WeaponRuntime target)
    {
        target.Damage = Damage;
        target.Cooldown = Cooldown;
    }
}
