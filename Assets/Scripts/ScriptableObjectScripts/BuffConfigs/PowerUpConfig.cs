using System;
using UnityEngine;

[Serializable]
public enum BuffType
{
    None,
    Health,
    Nitro,
    Shield
}

public abstract class PowerUpConfig : ScriptableObject
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private float duration;

    public BuffType BuffType => buffType;
    public float Duration => duration;

    public abstract BuffDefinition CreateDefinition();

}
