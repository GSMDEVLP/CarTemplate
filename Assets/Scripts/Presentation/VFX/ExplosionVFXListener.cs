using System.Collections;
using UnityEngine;

public class ExplosionVFXListener : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab; 
    [SerializeField] private AudioClip sfx;
    private IEventBus _bus;

    private IEnumerator Start()
    {
        while (CompositionRoot.Instance == null) yield return null;
        _bus = CompositionRoot.Instance.Events;
        _bus.Subscribe<Explosion>(OnExplosion);
    }
    void OnDestroy()
    {
        if (_bus != null)
            _bus.Unsubscribe<Explosion>(OnExplosion); 
    }

    void OnExplosion(Explosion e)
    {
        if (vfxPrefab) Instantiate(vfxPrefab, e.Position, Quaternion.identity);
        if (sfx) AudioSource.PlayClipAtPoint(sfx, e.Position);
    }
}
