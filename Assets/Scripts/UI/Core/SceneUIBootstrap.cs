using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UISceneKind
{
    Game,
    Menu
}
public class SceneUIBootstrap : MonoBehaviour
{
    [SerializeField] private UISceneKind uiKind = UISceneKind.Game;
    [SerializeField] private bool unloadOtherUi = true;

    private string gameUiScene = "UI_Game";
    private string menuUiScene = "UI_Menu";
    private void Awake()
    {
        var target = uiKind == UISceneKind.Game ? gameUiScene : menuUiScene;
        var other = uiKind == UISceneKind.Game ? menuUiScene : gameUiScene;

        if (unloadOtherUi && IsLoaded(other))
            SceneManager.UnloadSceneAsync(other);

        if (!IsLoaded(target))
            SceneManager.LoadSceneAsync(target, LoadSceneMode.Additive);
    }

    private static bool IsLoaded(string sceneName)
    {
        var s = SceneManager.GetSceneByName(sceneName);
        return s.IsValid() && s.isLoaded;
    }
}
