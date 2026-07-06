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

    public virtual float TimeLimit => 5f;

    public GameState CurrentState => _gameState;

    private void Start()
    {
        _uiManager.Initialize();

        _uiManager.UpdateLife(_life);
        _uiManager.UpdateRound(0);
        _uiManager.UpdateTimeUI(_elapsedTime);

        StartCoroutine(ReadyCoroutine());
    }


    private void StartGame()
    {
        Debug.Log("GameStart");
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

    private void EndGame()
    {
        Debug.Log("GameEnd");
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
            StartCoroutine(FinishSequence(MinigameResult.Success));
        }
        else
        {
            _life--;
            _uiManager.UpdateLife(_life);
            StartCoroutine(FinishSequence(MinigameResult.Failure));

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

    private IEnumerator GameTimer()
    {
        gameTime = 0f;
        while (gameTime < _elapsedTime)
        {
            gameTime += Time.deltaTime;

            float remain = _elapsedTime - gameTime;
            _uiManager.UpdateTimeUI(remain);

            yield return null;
        }
        HandleGameFinished(MinigameResult.Failure);
    }

    private IEnumerator FinishSequence(MinigameResult result)
    {
        _uiManager.ShowResult(result);

        yield return new WaitForSeconds(1f);

        _uiManager.HideResult();
    }

    private IEnumerator IntervalCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _uiManager.HideResult();
        StartCoroutine(ReadyCoroutine());
    }

    /// <summary>
    /// ゲーム開始のカウントダウン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadyCoroutine()
    {
        ChangeState(GameState.Ready);
        yield return _uiManager.PlayCountdown();
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
