using UnityEngine;

public sealed class PlayerWeaponProvider : MonoBehaviour
{
    public static PlayerWeaponProvider Instance { get; private set; }

    [SerializeField] private PlayerWeaponController controller;
    public PlayerWeaponController Controller => controller;

    private void Awake()
    {
        Instance = this;
        if (controller == null)
            controller = GetComponent<PlayerWeaponController>();
    }
}
