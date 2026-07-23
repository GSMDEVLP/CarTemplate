using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NitroBuffConfig", menuName = "Buff/NitroBuffConfig")]
public class NitroBuffConfig : PowerUpConfig
{
    [SerializeField] private float _boostPower;
    [SerializeField] private float _maxSpeed;

    public float BoostPower => _boostPower;
    public float MaxSpeed => _maxSpeed;

    public override BuffDefinition CreateDefinition()
    {
        return new NitroBuffDefinition(BoostPower, MaxSpeed, Duration);
    }
}

