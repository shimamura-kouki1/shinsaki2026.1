using Unity.VisualScripting;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private float _bGMvolume = 0.4f; 
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMNames.Title,_bGMvolume);
    }

    public void OnClickStart()
    {
        AudioManager.Instance.PlaySE(SENames.ButtonClick);
        AudioManager.Instance.StopBGM();
        SceneLoader.LoadGame();
    }

    public void OnClickExit()
    {
        AudioManager.Instance.PlaySE(SENames.ButtonBack);
    }
}
