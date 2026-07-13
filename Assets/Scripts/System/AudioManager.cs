using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

    [SerializeField] private List<AudioData> _seList;

    private Dictionary<string, AudioClip> _seDictionary;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _seDictionary = new Dictionary<string, AudioClip>();

        foreach (var se in _seList)
        {
            _seDictionary[se.Name] = se.Clip;
        }
    }

    public void PlaySE(string seName)
    {
        if (_seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            _seSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SEが登録されていません : {seName}");
        }
    }
}

public static class SENames
{
    public const string Click = "Click";
    public const string Success = "Success";
    public const string Failure = "Failure";
    public const string Countdown = "Countdown";
    public const string Start = "Start";
    public const string GameClear = "GameClear";
    public const string GameOver = "GameOver";
}
