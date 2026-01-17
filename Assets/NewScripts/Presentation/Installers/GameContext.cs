using UnityEngine;

public sealed class GameContext : MonoBehaviour
{
    [Header("Global Services")]
    [SerializeField] private LayerMask enemyLayer = 1 << 9;
    [SerializeField] private bool useLineOfSight = false;

    [Header("Installers")]
    [SerializeField] private MonoBehaviour[] installers;

    public static GameContext Instance { get; private set; }
    public GameServices Services => _services;
    private GameServices _services;

    private void Awake()
    {
        _services = new GameServices(enemyLayer, useLineOfSight);

        if (installers == null) return;

        for (int i = 0; i < installers.Length; i++)
        {
            if (installers[i] is IInstaller inst)
                inst.Install(_services);
        }
    }
}
