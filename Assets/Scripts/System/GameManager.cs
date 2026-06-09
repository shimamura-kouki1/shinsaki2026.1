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

    private BaseMinigame _currentGame;

    private Coroutine _timerCoroutine;

    private int _currentIndex;
    private int _clearCount;


    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        if (_clearCount >= _gameClearCount)
        {
            GameClear();
            return;
        }

        if (_miniGames.Count == 0)
        {
            Debug.LogError("ミニゲームが設定されていません。");
            return;
        }

        //ランダムにミニゲームを選択して開始
        _currentIndex = Random.Range(0, _miniGames.Count);
        _currentGame = _miniGames[_currentIndex];
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

    private IEnumerator GameTimer()
    {
        gameTime = 0f;
        while (gameTime < _elapsedTime)
        {
            gameTime += Time.deltaTime;
            yield return null;
        }
        EndGame();
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

        if (result == MinigameResult.Clear)
        {
            _clearCount++;
            Debug.Log($"クリア回数 : {_clearCount}");
        }
        Debug.Log($"結果 : {result}");



        EndGame();
    }

    private void Interval()
    {
        // インターバル処理（例: 2秒待機）
        StartCoroutine(IntervalCoroutine());
    }

    private IEnumerator IntervalCoroutine()
    {
        Debug.Log("インターバル開始");
        yield return new WaitForSeconds(2f);
        Debug.Log("インターバル終了");
        StartGame();
    }

    private void GameClear()
    {
        Debug.Log("ゲームクリア！");
    }

    private void GameOver()
    {
        Debug.Log("ゲームオーバー！");
    }
}
