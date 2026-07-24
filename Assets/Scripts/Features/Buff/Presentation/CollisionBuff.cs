using UnityEngine;

public class CollisionBuff : MonoBehaviour, ITriggerEnterHandler
{
    [SerializeField] private EntityIdComponent _entityId;
    private BuffService _service;
    public void Init(BuffService service)
    {
        _service = service;
    }

    public void OnTriggerEntered(Collider other)
    {
        if (_service == null || _entityId == null)
            return;

        if (!other.CompareTag("Buff"))
            return;

        var pickup = other.GetComponent<BuffPickup>();

        if (pickup == null)
            return;

        if (_service.TryApply(pickup.BuffType, _entityId.Id))
            pickup.Consume();
    }

}
