using UnityEngine;

public sealed class FeedbackInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CombatVfxEmitter vfxEmitter;
    [SerializeField] private CombatSfxEmitter sfxEmitter;
    [SerializeField] private VfxSystem vfxSystem;
    [SerializeField] private SfxSystem sfxSystem;
    [SerializeField] private CameraImpulse cameraImpulse;

    public void Install(GameServices services)
    {
        if (vfxEmitter != null) vfxEmitter.Init(services.Events);
        if (sfxEmitter != null) sfxEmitter.Init(services.Events);
        if (vfxSystem != null) vfxSystem.Init(services.Events);
        if (sfxSystem != null) sfxSystem.Init(services.Events);
        if (cameraImpulse != null) cameraImpulse.Init(services.Events);
    }
}
