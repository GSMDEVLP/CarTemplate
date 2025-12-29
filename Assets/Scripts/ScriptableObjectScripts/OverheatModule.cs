using UnityEngine;

[CreateAssetMenu(fileName ="OverheatModule", menuName = "Weapons/Modules/Overheat")]
public class OverheatModule : WeaponModule
{
    [SerializeField] private float _maxHeat = 100f;
    [SerializeField] private float _heatPerShot = 5f;
    [SerializeField] private float _coolRatePerSec = 15f;

    public float MaxHeat => _maxHeat;
    public float HeatPerShot => _heatPerShot;
    public float CoolRatePerSec => _coolRatePerSec;

    public override void Apply(WeaponRuntime target)
    {
        target.MaxHeat = MaxHeat;
        target.HeatPerShot  = HeatPerShot;
        target.CoolRatePerSec = CoolRatePerSec;
    }
}

