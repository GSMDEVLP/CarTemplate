using UnityEngine;

[CreateAssetMenu(fileName ="OverheatModule", menuName = "Weapons/Modules/Overheat")]
public class OverheatModule : WeaponModule
{
    public float MaxHeat = 100f;
    public float HeatPerShot = 5f;
    public float CoolRatePerSec = 15f;

    public override void Apply(WeaponRuntime target)
    {
        target.MaxHeat = MaxHeat;
        target.HeatPerShot  = HeatPerShot;
        target.CoolRatePerSec = CoolRatePerSec;
    }
}

