using UnityEngine;

public sealed class PlayerVehicleProvider : MonoBehaviour
{
    public static PlayerVehicleProvider Instance { get; private set; }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Ashsvp.GearSystem gearSystem;
    [SerializeField] private GameObject _damageResolve;
    
    public Rigidbody Rigidbody => rb;
    public Ashsvp.GearSystem GearSystem => gearSystem;
    public ITakesDamage DamageResolve => _damageResolve.GetComponent<ITakesDamage>();

    private void Awake()
    {
        Instance = this;
    }
}
