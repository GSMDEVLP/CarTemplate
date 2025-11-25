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
    public string ID;
    public WeaponKind Type;

    public FireMode FireMode;
    public WeaponMount WeaponMount;

    public List<WeaponModule> Modules = new();
}
