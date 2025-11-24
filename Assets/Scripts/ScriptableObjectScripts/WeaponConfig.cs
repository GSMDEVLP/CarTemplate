using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponKind
{
    Straight,
    Homing,
    Mine,
    Oil
}


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 0)]
public class WeaponConfig : ScriptableObject
{
    public string ID;
    public WeaponKind Type;

    public List<WeaponModule> Modules = new();
}
