using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ゲームの動作時間
    public float gameTime;

    [Header("ミニゲーム一覧"), SerializeField]
    private List<BaseMinigame> _miniGames;

    [Header("制限時間"), SerializeField]
    private float _elapsedTime = 5f;

    [SerializeField] private int _gameClearCount = 3;
    [SerializeField] private int _life = 3;

    [SerializeField] private GameUIManager _uiManager;
    [SerializeField] private float _bGMVolume = 0.4f;

    private BaseMinigame _currentGame;
    // ミニゲームをランダムな順番で管理するリスト
    private List<BaseMinigame> _shuffleGames = new();
    // シャッフル済みリストの現在の位置
    private int _shuffleIndex;
    private GameState _gameState;

    private Coroutine _timerCoroutine;

    private int _clearCount;
    private int _round;

    private MinigameResult _lastResult;
    public virtual float TimeLimit => 5f;

    public GameState CurrentState => _gameState;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMNames.InGame);

        _uiManager.Initialize();

        _uiManager.UpdateLife(_life);
        _uiManager.UpdateRound(0);
        _uiManager.UpdateTimeUI(_elapsedTime);

        ShuffleMinigames();

        StartCoroutine(ReadyCoroutine());
    }


    private void StartRound()
    {
        _round++;
        _uiManager.UpdateRound(_round);

        if (!_currentGame) return;

        //ChangeState(GameState.Playing);

        _currentGame.gameObject.SetActive(true);
        _currentGame.StartGame();

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _currentGame.OnGameFinished += HandleGameFinished;

        // 制限時間のカウントダウンを開始
        _timerCoroutine = StartCoroutine(GameTimer());
    }

    /// <summary>
    /// ミニゲーム一覧をシャッフルする
    /// 1周するまでは同じゲームが出ない
    /// </summary>
    private void ShuffleMinigames()
    {
        // 元のリストをコピー
        _shuffleGames = new List<BaseMinigame>(_miniGames);

        // Fisher-Yatesシャッフル
        for (int i = _shuffleGames.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            (_shuffleGames[i], _shuffleGames[j]) =
                (_shuffleGames[j], _shuffleGames[i]);
        }

        // 先頭から取り出せるようにインデックスをリセット
        _shuffleIndex = 0;
    }

    /// <summary>
    /// 次ミニゲームの選択
    /// </summary>
    private void BeginRound()
    {

        if (_miniGames.Count == 0)
        {
            Debug.LogError("ミニゲームが設定されていません。");
            return;
        }


        // 全部遊び終わったらもう一度シャッフル
        if (_shuffleIndex >= _shuffleGames.Count)
        {
            ShuffleMinigames();
        }

        // シャッフル済みリストから次のゲームを取得
        _currentGame = _shuffleGames[_shuffleIndex];
        _shuffleIndex++;
    }

    private void EndGame()
    {
        Debug.Log("GameEnd");
        StopCurrentGame();
        _uiManager.UpdateTimeUI(_elapsedTime);
        _uiManager.HideInstruction();
        Interval();
        //StartGame(); // 次のゲームを開始
    }

    private void StopCurrentGame()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
        if (_currentGame != null)
        {
            _currentGame.OnGameFinished -= HandleGameFinished;
            _currentGame.EndGame();
            _currentGame.gameObject.SetActive(false);
            _currentGame = null;
        }
    }

    /// <summary>
    /// ミニゲーム終了時の処理
    /// </summary>
    private void HandleGameFinished(MinigameResult result)
    {
        if (_currentGame == null) return;

        _lastResult = result;

        // 成功時
        if (result == MinigameResult.Success)
        {
            AudioManager.Instance.PlaySE(SENames.MinigameClear);

            _clearCount++;

            // 規定回数クリアでゲームクリア
            if (_clearCount >= _gameClearCount)
            {
                GameClear();
                return;
            }
        }
        // 失敗時
        else
        {
            AudioManager.Instance.PlaySE(SENames.MinigameFailed);

            _life--;
            _uiManager.UpdateLife(_life);

            if (_life <= 0)
            {
                GameOver();
                return;
            }
        }

        EndGame();
    }

    private void Interval()
    {
        Debug.Log("Interval2");
        ChangeState(GameState.Interval);
        // インターバル処理（例: 2秒待機）
        StartCoroutine(IntervalCoroutine());
    }

    /// <summary>
    /// 制限時間を管理する
    /// </summary>
    private IEnumerator GameTimer()
    {
        gameTime = 0f;
        while (gameTime < _elapsedTime)
        {
            gameTime += Time.deltaTime;

            float remain = _elapsedTime - gameTime;
            // 残り時間をUIへ反映
            _uiManager.UpdateTimeUI(remain);

            yield return null;
        }

        // 時間切れ
        HandleGameFinished(MinigameResult.Failure);
    }

    private IEnumerator IntervalCoroutine()
    {
        _uiManager.ShowResult(_lastResult);

        yield return new WaitForSeconds(1f);

        _uiManager.HideResult();

        yield return new WaitForSeconds(1f);

        StartCoroutine(ReadyCoroutine());
    }

    /// <summary>
    /// ゲーム開始のカウントダウン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadyCoroutine()
    {
        ChangeState(GameState.Ready);

        // 次のミニゲームを選択
        BeginRound();

        if (!_currentGame)
        {
            yield break;
        }

        // ゲーム説明を表示
        _uiManager.ShowInstruction(
       _currentGame.Title,
       _currentGame.Description);

        AudioManager.Instance.PlaySE(SENames.Countdown);

        // カウントダウン演出
        yield return _uiManager.PlayCountdown();

        //AudioManager.Instance.PlaySE(SENames.GameStart);現在SEがない

        _uiManager.HideInstruction();

        // ミニゲーム開始
        StartRound();
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    private void GameClear()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE(SENames.GameClear);
        AudioManager.Instance.PlayBGM(BGMNames.Result, _bGMVolume);

        ChangeState(GameState.GameClear);
        StopCurrentGame();
        SceneLoader.LoadGameClear();
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void GameOver()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE(SENames.GameOver);
        AudioManager.Instance.PlayBGM(BGMNames.Result, _bGMVolume);

        ChangeState(GameState.GameOver);

        StopCurrentGame();
        SceneLoader.LoadGameOver();
    }

    private void ChangeState(GameState gameState)
    {
        if (_gameState == gameState)
            return;

        _gameState = gameState;
    }
}
