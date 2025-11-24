using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileModule", menuName = "Weapons/Modules/Projectile")]
public class ProjectileModule : WeaponModule
{
    public GameObject Prefab;
    public float Speed;
    public float LifeTime;

    public override void Apply(WeaponRuntime target)
    {
        target.ProjectilePrefab = Prefab;
        target.Speed = Speed;
        target.LifeTime = LifeTime;
    }
}