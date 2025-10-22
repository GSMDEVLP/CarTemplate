using UnityEngine;

public class FlippedDetected : IEvent
{
    public readonly Object Sender;
    public FlippedDetected(Object sender)
    {
        Sender = sender; 
    }
}
