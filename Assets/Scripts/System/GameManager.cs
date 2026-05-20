using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("ミニゲーム一覧"),SerializeField] 
    private List<BaseMinigame> _miniGames;

    [Header("制限時間"), SerializeField]
    private float _gameTime = 5f;

    private BaseMinigame _currentGame;

    private Coroutine _timerCoroutine;

    private int _currentIndex;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
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
        float elapsedTime = 0f;
        while (elapsedTime < _gameTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EndGame();
    }

    private void EndGame()
    {
        if (_currentGame != null)
        {
            _currentGame.OnGameFinished -= HandleGameFinished;
            _currentGame.EndGame();
            _currentGame = null;
        }
        StartGame(); // 次のゲームを開始
    }

    private void HandleGameFinished(MinigameResult result)
    {
        Debug.Log($"結果 : {result}");

        EndGame();
    }
}
