using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 0)]
public class WeaponConfig : ScriptableObject
{
    public int CoolDown;
    public int Damage;
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float HomingStrength;
    public float LifeTime;

}
