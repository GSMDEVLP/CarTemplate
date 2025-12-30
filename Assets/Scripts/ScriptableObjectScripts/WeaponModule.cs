using UnityEngine;

public class WeaponRuntime
{
    public GameObject ProjectilePrefab;

    public float Cooldown;
    public float Damage;

    public float Speed;
    public float LifeTime;

    public float HomingStrength;
    public float SeekRadius;

    public float ExplosionRadius;
    public float ArmingDelay;

    public float MaxHeat;          
    public float HeatPerShot;      
    public float CoolRatePerSec;   

    public int MaxAmmo;
    public int CurrentAmmo;

}

public abstract class WeaponModule: ScriptableObject
{
    public abstract void Apply(WeaponRuntime target);
}