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
    [SerializeField] private PowerUpType _type;
    [SerializeField] private float _duration;
    [SerializeField] private int _value;

    public PowerUpType Type => _type;
    public float Duration => _duration;
    public int Value => _value;
}
