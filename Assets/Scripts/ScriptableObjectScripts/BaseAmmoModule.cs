
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAmmoModule", menuName = "Weapons/Modules/Ammo")]
public class BaseAmmoModule : WeaponModule
{
    [SerializeField] private int _maxAmmo;

    public override void Apply(WeaponRuntime target)
    {
        target.MaxAmmo = _maxAmmo;
        target.CurrentAmmo = _maxAmmo;
    }
}
