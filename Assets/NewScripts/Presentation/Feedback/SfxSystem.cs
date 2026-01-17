using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxSystem : MonoBehaviour
{
    [SerializeField] private SfxLibrary _library;
    [SerializeField] private int _poolSize = 8;
    [SerializeField] private bool _allowExpand = true;

    private IEventBus _bus;
    private readonly List<AudioSource> _sources = new List<AudioSource>();

    public void Init(IEventBus bus)
    {
        _bus = bus;
        _bus.Subscribe<SfxRequest>(OnSfxRequest);

        for (int i = 0; i < _poolSize; i++)
            _sources.Add(CreateSource());
    }

    private void OnDestroy()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<SfxRequest>(OnSfxRequest);
    }

    private void OnSfxRequest(SfxRequest e)
    {
        if (_library == null || e.Id == SfxId.None) return;
        if (!_library.TryGet(e.Id, out var entry)) return;

        var clip = PickClip(entry.Clips);
        if (clip == null) return;

        var source = GetSource();
        if (source == null) return;

        source.outputAudioMixerGroup = entry.Output;
        source.volume = entry.Volume * e.Volume;

        float pitch = Random.Range(entry.PitchRange.x, entry.PitchRange.y);
        if (Mathf.Approximately(pitch, 0f)) pitch = 1f;
        pitch *= e.Pitch;
        source.pitch = pitch;

        bool is2D = e.Is2D || !entry.Spatial;
        source.spatialBlend = is2D ? 0f : 1f;
        if (entry.MinDistance > 0f) source.minDistance = entry.MinDistance;
        if (entry.MaxDistance > 0f) source.maxDistance = entry.MaxDistance;

        if (entry.AttachToParent && e.Parent != null)
        {
            source.transform.SetParent(e.Parent, false);
            source.transform.localPosition = Vector3.zero;
        }
        else
        {
            source.transform.SetParent(transform, false);
            source.transform.position = is2D ? transform.position : e.Position;
        }

        source.loop = false;
        source.clip = clip;
        source.Play();

        float duration = clip.length / Mathf.Abs(source.pitch);
        StartCoroutine(ReleaseAfter(source, duration));
    }

    private AudioSource CreateSource()
    {
        var go = new GameObject("SfxSource");
        go.transform.SetParent(transform, false);
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        return src;
    }

    private AudioSource GetSource()
    {
        for (int i = 0; i < _sources.Count; i++)
            if (!_sources[i].isPlaying)
                return _sources[i];

        if (_allowExpand)
        {
            var src = CreateSource();
            _sources.Add(src);
            return src;
        }

        return null;
    }

    private static AudioClip PickClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }

    private IEnumerator ReleaseAfter(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (source == null) yield break;
        source.Stop();
        source.clip = null;
        source.transform.SetParent(transform, false);
    }
}
