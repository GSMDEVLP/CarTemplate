using UnityEngine;

public sealed class PlayerVehicleProvider : MonoBehaviour
{
    public static PlayerVehicleProvider Instance { get; private set; }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Ashsvp.GearSystem gearSystem;
    
    public Rigidbody Rigidbody => rb;
    public Ashsvp.GearSystem GearSystem => gearSystem;

    private void Awake()
    {
        Instance = this;
    }
}
