using System;

public interface IVehicleTelemetrySource
{
    float SpeedKph { get; }
    int CurrentGear { get; }

    event Action<int> GearChanged;
}
