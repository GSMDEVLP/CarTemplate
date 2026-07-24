using UnityEngine;

public abstract class PowerUpConfig : ScriptableObject
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private float duration;

    public BuffType BuffType => buffType;
    public float Duration => duration;

    public abstract BuffDefinition CreateDefinition();

}
