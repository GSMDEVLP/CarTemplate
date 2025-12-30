using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
    Single, 
    Auto    
}
public enum WeaponKind
{
    Straight,
    Homing,
    MachineGun,
    Mine,
    Oil
}

public enum WeaponMount
{
    Front,
    Roof,
    Rear
}


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 0)]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;    
    [SerializeField] private WeaponKind _type;
    [SerializeField] private FireMode _fireMode;
    [SerializeField] private WeaponMount _weaponMount;
    [SerializeField] private List<WeaponModule> _modules;

    public string ID => _id;
    public Sprite Icon => _icon;
    public WeaponKind Type => _type;
    public FireMode FireMode => _fireMode;
    public WeaponMount WeaponMount => _weaponMount;
    public List<WeaponModule> Modules => _modules;
}
