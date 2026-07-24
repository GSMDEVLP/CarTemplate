using AshDev.Utility;
using UnityEngine;

public class BuffHUB : MonoBehaviour
{
    [SerializeField] private NitroBoost _nitroBoost;
    [SerializeField] private VehicleShield _vehicleShield;

    public void ActivateNitro(float boostPower, float maxSpeed, float duration)
    {
        _nitroBoost?.ActivateNitro(boostPower, maxSpeed, duration);
    }

    public void ActivateShield(float endurance, float duration)
    {
        _vehicleShield?.ActivateShield(endurance, duration);
    }
}
