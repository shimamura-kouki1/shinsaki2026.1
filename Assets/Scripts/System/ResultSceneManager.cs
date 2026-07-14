using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    public void OnRetry()
    {
        AudioManager.Instance.PlaySE(SENames.ButtonClick);
        SceneLoader.LoadGame();
    }

    public void OnBackTitle()
    {
        AudioManager.Instance.PlaySE(SENames.ButtonBack);
        SceneLoader.LoadTitle();
    }
}
