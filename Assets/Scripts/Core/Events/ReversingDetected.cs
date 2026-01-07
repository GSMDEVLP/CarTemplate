using UnityEngine;

public readonly struct ReversingDetected : IEvent
{
    public readonly Object Sender;
    public ReversingDetected(Object sender)
    {
        Sender = sender; 
    }
}
