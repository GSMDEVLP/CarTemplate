using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponKind
{
    Straight,
    Homing,
    Mine,
    Oil
}


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 0)]
public class WeaponConfig : ScriptableObject
{
    public string ID;                
    public WeaponKind Kind;
    public float Cooldown = 0.5f;
    public float Damage = 20f;

    [Header("Projectile")]
    public GameObject ProjectilePrefab;
    public float Speed = 30f;
    public float LifeTime = 5f;

    [Header("Homing")]
    [Range(0,1)] public float HomingStrength = 0.6f;
    public float SeekRadius = 40f;

    [Header("Mine/Oil")]
    public float ArmingDelay = 0.3f;
    public float ExplosionRadius = 6f;
}
