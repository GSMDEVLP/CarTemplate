using UnityEngine;

public sealed class BuffPickup : MonoBehaviour
{
    [SerializeField] private BuffType _buffType;

    private bool _consumed;

    public BuffType BuffType => _buffType;

    public void Consume()
    {
        if (_consumed)
            return;

        _consumed = true;
        Destroy(gameObject);
    }
}