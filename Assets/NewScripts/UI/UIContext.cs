using System.Collections;
using UnityEngine;

public sealed class UIContext : MonoBehaviour
{
    [SerializeField] private UIInstaller installer;

    private IEnumerator Start()
    {
        while (GameContext.Instance == null)
            yield return null;

        installer.Install(GameContext.Instance.Services);
    }
}
