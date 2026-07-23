using AshDev.Utility;
using UnityEngine;

public class BuffHUB : MonoBehaviour
{
    [SerializeField] private NitroBoost _nitroBoost;
    public NitroBoost nitroBoost => _nitroBoost;

    public void ActivateBoost(float boostPower, float maxSpeed, float duration)
    {
        _nitroBoost.ActivateBoost(boostPower, maxSpeed, duration);
    }
}
