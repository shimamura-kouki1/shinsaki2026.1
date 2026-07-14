using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

    [SerializeField] private List<AudioData> _seList;
    [SerializeField] private List<AudioData> _bgmList;
    [SerializeField] private const float _seVolume = 0.6f;
    [SerializeField] private const float _bgmVolume = 0.4f;

    private Dictionary<string, AudioClip> _seDictionary;
    private Dictionary<string, AudioClip> _bgmDictionary;

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

        _bgmDictionary = new Dictionary<string, AudioClip>();

        foreach (var bgm in _bgmList)
        {
            _bgmDictionary[bgm.Name] = bgm.Clip;
        }
    }

    public void PlaySE(string seName, float volume = _seVolume)
    {
        if (_seDictionary.TryGetValue(seName, out AudioClip clip))
        {
            _seSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"SEが登録されていません : {seName}");
        }
    }

    public void PlayBGM(string bgmName, float volume = _bgmVolume)
    {
        if (_bgmDictionary.TryGetValue(bgmName, out AudioClip clip))
        {
            if (_bgmSource.clip == clip && _bgmSource.isPlaying)
                return;

            _bgmSource.clip = clip;
            _bgmSource.volume = volume;
            _bgmSource.loop = true;
            _bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }
}

public static class SENames
{
    // UI
    public const string ButtonClick = "ButtonClick";
    public const string ButtonBack = "ButtonBack";

    // システム
    public const string Intro = "Intro";
    public const string Countdown = "Countdown";
    public const string GameStart = "GameStart";
    public const string MinigameClear = "MinigameClear";
    public const string MinigameFailed = "MinigameFailed";
    public const string GameClear = "GameClear";
    public const string GameOver = "GameOver";

    // ライツアウト
    public const string PanelPush = "PanelPush";

    // パスワード解析
    public const string KeyInput = "KeyInput";
    public const string WrongInput = "WrongInput";

    // ケーブル接続
    public const string CableGrab = "CableGrab";
    public const string CableConnect = "CableConnect";

    // 接続維持
    public const string Warning = "Warning";

    public const string Clear = "GameClear";
}

public static class BGMNames
{
    public const string Title = "Title";
    public const string InGame = "InGame";
    public const string Result = "Result";
}