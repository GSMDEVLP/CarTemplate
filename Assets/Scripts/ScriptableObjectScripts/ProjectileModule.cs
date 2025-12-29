using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileModule", menuName = "Weapons/Modules/Projectile")]
public class ProjectileModule : WeaponModule
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    
    public GameObject Prefab => _prefab;
    public float Speed => _speed;
    public float LifeTime => _lifeTime;

    public override void Apply(WeaponRuntime target)
    {
        target.ProjectilePrefab = Prefab;
        target.Speed = Speed;
        target.LifeTime = LifeTime;
    }
}