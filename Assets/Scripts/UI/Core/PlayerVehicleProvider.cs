using System.Diagnostics;
using UnityEngine;

public sealed class PlayerVehicleProvider : MonoBehaviour
{
    public static PlayerVehicleProvider Instance { get; private set; }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Ashsvp.GearSystem gearSystem;
    [SerializeField] private GameObject _damageResolve;
    [SerializeField] private EntityIdComponent _entityIdComponent;
    
    public Rigidbody Rigidbody => rb;
    public Ashsvp.GearSystem GearSystem => gearSystem;
    public ITakesDamage DamageResolve => _damageResolve.GetComponent<ITakesDamage>();
    public EntityIdComponent EntityIdComponent => _entityIdComponent;

    private void Awake()
    {
        Instance = this;
    }
}
