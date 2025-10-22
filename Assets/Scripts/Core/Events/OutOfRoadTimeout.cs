using UnityEngine;

public class OutOfRoadTimeout : IEvent
{
    public readonly Object Sender;
    public readonly float OffTime;
    public OutOfRoadTimeout(Object sender, float offTime)
    {
        Sender = sender;    
        OffTime = offTime;
    }

}
