using UnityEngine;

public readonly struct RespawnPerformed : IEvent
{
    public readonly Object Target;
    public RespawnPerformed(Object target)
    {
        Target = target; 
    }
}