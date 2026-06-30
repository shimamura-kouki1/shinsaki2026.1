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

    private BaseMinigame _currentGame;
    private GameState _gameState;

    private Coroutine _timerCoroutine;

    private int _currentIndex;
    private int _clearCount;
    private int _round;

    public GameState CurrentState => _gameState;

    private void Start()
    {
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

        BeginRound();

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
            yield return null;
        }
        HandleGameFinished(MinigameResult.Failure);
    }

    private void EndGame()
    {
       
        if(_timerCoroutine != null)
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
        Interval();
        //StartGame(); // 次のゲームを開始
    }

    private void HandleGameFinished(MinigameResult result)
    {
        if(_currentGame == null) return;

        if (result == MinigameResult.Success)
        {
            _clearCount++;
            Debug.Log($"クリア回数 : {_clearCount}");
        }
        else
        {
            _life--;

            if (_life <= 0)
            {
                GameOver();
                return;
            }
        }
        Debug.Log($"結果 : {result}");
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
        //ここにUIを入れる
        ChangeState(GameState.Ready);
        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        Debug.Log("Start");
        StartGame();
    }

    private void GameClear()
    {
        ChangeState(GameState.GameClear);
        Debug.Log("ゲームクリア！");
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void GameOver()
    {
        ChangeState(GameState.GameOver);

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

        Debug.Log("ゲームオーバー！");
    }

    private void ChangeState(GameState gameState)
    {
        if (_gameState == gameState)
        return;

    _gameState = gameState;
    }
}
