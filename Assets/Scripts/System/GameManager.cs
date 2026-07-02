using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ゲームの動作時間
    public float gameTime;

    [Header("ミニゲーム一覧"),SerializeField] 
    private List<BaseMinigame> _miniGames;

    [Header("制限時間"), SerializeField]
    private float _elapsedTime = 5f;

    [SerializeField] private int _gameClearCount = 3;
    [SerializeField] private int _life = 3;

    [SerializeField] private GameUIManager _uiManager;

    private BaseMinigame _currentGame;
    private GameState _gameState;

    private Coroutine _timerCoroutine;

    private int _currentIndex;
    private int _clearCount;
    private int _round;

    public GameState CurrentState => _gameState;

    private void Start()
    {
        _uiManager.UpdateLife(_life);
        _uiManager.UpdateRound(0);
        _uiManager.UpDateTimeUI(_elapsedTime);

        StartCoroutine(ReadyCoroutine());
    }

    private void StartGame()
    {
        if (_clearCount >= _gameClearCount)
        {
            GameClear();
            return;
        }

        _round++;
        _uiManager.UpdateRound(_round);

        BeginRound();

        if (!_currentGame) return;

        _uiManager.ShowInstruction(_currentGame.Title, _currentGame.Description);

        ChangeState(GameState.Playing);

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
    /// ミニゲームの選択
    /// </summary>
    private void BeginRound()
    {

        if (_miniGames.Count == 0)
        {
            Debug.LogError("ミニゲームが設定されていません。");
            return;
        }

        //ランダムにミニゲームを選択して開始
        _currentIndex = Random.Range(0, _miniGames.Count);
        _currentGame = _miniGames[_currentIndex];
    }

    private IEnumerator GameTimer()
    {
        gameTime = 0f;
        while (gameTime < _elapsedTime)
        {
            gameTime += Time.deltaTime;

            float remain = _elapsedTime - gameTime;
            _uiManager.UpDateTimeUI(remain);

            yield return null;
        }
        HandleGameFinished(MinigameResult.Failure);
    }

    private void EndGame()
    {
        StopCurrentGame();
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

    private void HandleGameFinished(MinigameResult result)
    {
        if(_currentGame == null) return;

        if (result == MinigameResult.Success)
        {
            _clearCount++;
            _uiManager.ShowResult(MinigameResult.Success);
        }
        else
        {
            _life--;
            _uiManager.UpdateLife(_life);
            _uiManager.ShowResult(MinigameResult.Failure);

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
        ChangeState(GameState.Interval);
        // インターバル処理（例: 2秒待機）
        StartCoroutine(IntervalCoroutine());
    }

    private IEnumerator IntervalCoroutine()
    {
        Debug.Log("インターバル開始");
        yield return new WaitForSeconds(2f);
        Debug.Log("インターバル終了");

        StartCoroutine(ReadyCoroutine());
    }

    /// <summary>
    /// ゲーム開始のカウントダウン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadyCoroutine()
    {
        ChangeState(GameState.Ready);
        _uiManager.ShowCountdown("3");
        yield return new WaitForSeconds(1f);
        _uiManager.ShowCountdown("2");
        yield return new WaitForSeconds(1f);
        _uiManager.ShowCountdown("1");
        yield return new WaitForSeconds(1f);
        _uiManager.ShowCountdown("GO");
        yield return new WaitForSeconds(0.5f);
        _uiManager.HideCountdown();
        StartGame();
    }

    private void GameClear()
    {
        ChangeState(GameState.GameClear);
        _uiManager.ShowResult(MinigameResult.Success);
        Debug.Log("ゲームクリア！");
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void GameOver()
    {
        ChangeState(GameState.GameOver);

        StopCurrentGame();
        _uiManager.ShowResult(MinigameResult.Failure);

        Debug.Log("ゲームオーバー！");
    }

    private void ChangeState(GameState gameState)
    {
        if (_gameState == gameState)
        return;

    _gameState = gameState;
    }
}
