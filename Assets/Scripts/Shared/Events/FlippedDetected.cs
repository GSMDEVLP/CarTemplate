using UnityEngine;

public readonly struct FlippedDetected : IEvent
{
    public readonly Object Sender;
    public FlippedDetected(Object sender)
    {
        Sender = sender; 
    }
}
