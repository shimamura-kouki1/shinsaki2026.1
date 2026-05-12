using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("ミニゲーム一覧"),SerializeField] 
    private List<BaseMinigame> _miniGames;

    [Header("制限時間"), SerializeField]
    private float _gameTime = 5f;

    private BaseMinigame _currentGame;

    private Coroutine _timerCoroutine;

    private int _currentIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void StartGame()
    {
        if (_miniGames.Count == 0)
        {
            Debug.LogError("ミニゲームが設定されていません。");
            return;
        }
        _currentIndex = Random.Range(0, _miniGames.Count);
        _currentGame = _miniGames[_currentIndex];
        _currentGame.StartGame();
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        _timerCoroutine = StartCoroutine(GameTimer());
    }

    private IEnumerator<GameObject> GameTimer()
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
            _currentGame.EndGame();
            _currentGame = null;
        }
        StartGame(); // 次のゲームを開始
    }
}
