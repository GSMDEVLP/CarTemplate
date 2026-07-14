using UnityEngine;

public sealed class PlayerHealthProvider : MonoBehaviour, IPlayerHealthSource
{
    public static PlayerHealthProvider Instance { get; private set; }

    [SerializeField] private GameObject damageResolve;
    [SerializeField] private EntityIdComponent entityIdComponent;

    private ITakesDamage _health;

    public EntityId EntityId => entityIdComponent != null ? entityIdComponent.Id : default;
    public float CurrentHealth => _health != null ? _health.CurrentHP : 0f;
    public float MaxHealth => _health != null ? _health.MaxHP : 0f;

    private void Awake()
    {
        Instance = this;
        if (damageResolve != null)
            _health = damageResolve.GetComponent<ITakesDamage>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
