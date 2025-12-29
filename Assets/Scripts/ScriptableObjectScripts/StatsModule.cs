using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsModule", menuName = "Weapons/Modules/Stats")]
public class StatsModule : WeaponModule
{
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;

    public float Damage => _damage;
    public float Cooldown => _cooldown;

    public override void Apply(WeaponRuntime target)
    {
        target.Damage = Damage;
        target.Cooldown = Cooldown;
    }
}
