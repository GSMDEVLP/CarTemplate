using System;
using UnityEngine;

[Serializable]
public class AIWeaponSlot
{
    public WeaponConfig Config;
    public int Priority = 0;
    public float MinRange = 0f;
    public float MaxRange = 120f;
    [Range(-1f, 1f)] public float MinDot = 0.5f;
    [Range(-1f, 1f)] public float MaxDot = 1f;
    public bool RequiresLineOfSight = true;
    public int BurstMinShots = 2;
    public int BurstMaxShots = 4;
    public float ShotInterval = 0.1f;
    public float BurstCooldownMin = 0.5f;
    public float BurstCooldownMax = 1.5f;
}
