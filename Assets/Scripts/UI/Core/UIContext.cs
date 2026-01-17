using System.Collections;
using UnityEngine;

public sealed class UIContext : MonoBehaviour
{
    [SerializeField] private UIInstaller installer;

    private IEnumerator Start()
    {
        Debug.Log("UIContext started");
        while (GameContext.Instance == null)
            yield return null;

        Debug.Log("UIContext GameContext.Instance is available");
        installer.Install(GameContext.Instance.Services);
    }
}
