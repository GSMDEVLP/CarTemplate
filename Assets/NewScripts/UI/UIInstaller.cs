using UnityEngine;

public sealed class UIInstaller : MonoBehaviour
{
    [SerializeField] private GameUIRoot gameUIRoot;

    public void Install(GameServices services)
    {
        if (gameUIRoot != null)
            gameUIRoot.Init(services.Events);
    }
}
