using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    public void OnRetry()
    {
        SceneLoader.LoadGame();
    }

    public void OnBackTitle()
    {
        SceneLoader.LoadTitle();
    }
}
