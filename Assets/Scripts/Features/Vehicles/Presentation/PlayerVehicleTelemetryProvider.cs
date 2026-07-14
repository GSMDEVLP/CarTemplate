using System;
using UnityEngine;

public sealed class PlayerVehicleTelemetryProvider : MonoBehaviour, IVehicleTelemetrySource
{
    public static PlayerVehicleTelemetryProvider Instance { get; private set; }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Ashsvp.GearSystem gearSystem;

    public float SpeedKph => rb != null ? rb.linearVelocity.magnitude * 3.6f : 0f;
    public int CurrentGear => gearSystem != null ? gearSystem.currentGear : 0;

    public event Action<int> GearChanged;

    private void Awake()
    {
        Instance = this;
        if (gearSystem != null)
            gearSystem.OnGearChanged += OnGearChanged;
    }

    private void OnGearChanged(int gear)
    {
        GearChanged?.Invoke(gear);
    }

    private void OnDestroy()
    {
        if (gearSystem != null)
            gearSystem.OnGearChanged -= OnGearChanged;

        if (Instance == this)
            Instance = null;
    }
}
