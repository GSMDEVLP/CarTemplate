using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpConfig", menuName = "Buff/ShieldBuffConfig")]
public class ShieldBuffConfig : PowerUpConfig
{
    [SerializeField] private float _shieldEndurance;

    public float ShieldEndurance => _shieldEndurance;
    public override BuffDefinition CreateDefinition()
    {
        return new ShieldBuffDefinition(ShieldEndurance, Duration);
    }
}
