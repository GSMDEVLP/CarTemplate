using UnityEngine;

public class UnityTimeService : ITime
{
    public float DeltaTime => Time.deltaTime;
    public float TimeSinceStartup => Time.time;
}
