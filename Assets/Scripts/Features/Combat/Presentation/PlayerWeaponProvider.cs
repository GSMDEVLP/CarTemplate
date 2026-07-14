using System;
using UnityEngine;

public sealed class PlayerWeaponProvider : MonoBehaviour
{
    public static PlayerWeaponProvider Instance { get; private set; }

    public IWeaponHudSource HudSource { get; private set; }
    public bool IsReady => HudSource != null;

    public void Initialize(IWeaponHudSource hudSource)
    {
        if (HudSource is IDisposable disposable)
            disposable.Dispose();

        HudSource = hudSource ?? throw new ArgumentNullException(nameof(hudSource));
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (HudSource is IDisposable disposable)
            disposable.Dispose();

        HudSource = null;
        if (Instance == this)
            Instance = null;
    }
}
