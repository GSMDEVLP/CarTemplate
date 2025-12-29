using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSlotState", menuName = "Weapons/Modules/WeaponSlot")]
public class WeaponSlotState : ScriptableObject
{
    [SerializeField] private string _ammoText;
    [SerializeField] private string _cooldownText;
    [SerializeField] private bool _isActive;
    [SerializeField] private Sprite _icon;

    public string AmmoText => _ammoText;
    public string CooldownText => _cooldownText;
    public bool IsActive => _isActive;
    public Sprite Icon => _icon;
}
