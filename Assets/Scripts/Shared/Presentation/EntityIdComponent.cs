using UnityEngine;

public sealed class EntityIdComponent : MonoBehaviour
{
    [SerializeField] private int id;
    public EntityId Id => new EntityId(id);

    private void Awake() => UnityEntityRegistry.Register(Id, gameObject);
    private void OnDestroy() => UnityEntityRegistry.Unregister(Id, gameObject);
}
