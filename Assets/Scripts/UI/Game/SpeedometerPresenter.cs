using UnityEngine;

public sealed class SpeedometerPresenter : MonoBehaviour
{
    [Header("Speed Source")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speedMultiplier = 1f;

    [Header("Gear Source")]
    [SerializeField] private Ashsvp.GearSystem gearSystem;

    [Header("View")]
    [SerializeField] private SpeedometerView view;

    [Header("Speedometer Settings")]
    [SerializeField] private float minAngle = -130f;
    [SerializeField] private float maxAngle = 130f;
    [SerializeField] private float maxSpeed = 200f;

    private SpeedometerViewModel _vm;

    private void Awake()
    {
        _vm = new SpeedometerViewModel(GetSpeed, minAngle, maxAngle, maxSpeed, gearSystem);
        view.Bind(_vm);
    }

    private void Update()
    {
        _vm.RefreshSpeed();
    }

    private float GetSpeed()
    {
        if (rb == null) return 0f;
        return rb.velocity.magnitude * speedMultiplier;
    }

    private void OnDestroy()
    {
        _vm.Dispose();
    }
}
