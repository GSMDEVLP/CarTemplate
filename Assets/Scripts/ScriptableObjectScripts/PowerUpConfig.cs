using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerUpType
{
    Nitro,
    Shield,
    Repair
}
[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObject/PowerUp", order = 1)]
public class PowerUpConfig : ScriptableObject
{
    public PowerUpType Type;
    public float Duretion;
    public int value;
}
