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
        if (vfxEmitter != null) vfxEmitter.Init(services.EventBus);
        if (sfxEmitter != null) sfxEmitter.Init(services.EventBus);
        if (vfxSystem != null) vfxSystem.Init(services.EventBus);
        if (sfxSystem != null) sfxSystem.Init(services.EventBus);
        if (cameraImpulse != null) cameraImpulse.Init(services.EventBus);
    }
}
