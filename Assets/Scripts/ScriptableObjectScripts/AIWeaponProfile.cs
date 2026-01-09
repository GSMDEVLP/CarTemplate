using UnityEngine;

[CreateAssetMenu(fileName = "AIWeaponProfile", menuName = "AI/Weapon Profile")]
public class AIWeaponProfile : ScriptableObject
{
    public bool RefreshAmmoWhenEmpty = true;
    public AIWeaponSlot[] Slots;
}
