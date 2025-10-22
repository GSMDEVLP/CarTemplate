using UnityEngine;

public class ReversingDetected : IEvent
{
    public readonly Object Sender;
    public ReversingDetected(Object sender)
    {
        Sender = sender; 
    }
}
