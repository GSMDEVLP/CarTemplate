using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpConfig", menuName = "Buff/PowerUpConfig")]
public class HealBuffConfig : PowerUpConfig
{
    [SerializeField] private float healAmount;

    public float HealAmount => healAmount;

    public override BuffDefinition CreateDefinition()
    {
        return new HealBuffDefinition(Duration, HealAmount);
    }
}
