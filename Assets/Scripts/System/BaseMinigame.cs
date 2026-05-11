using UnityEngine;
using System;

public abstract class BaseMinigame : MonoBehaviour, IMinigame
{
    public event Action<MinigameResult> OnGameFinished;

    public abstract void StartGame();

    protected void Finish(MinigameResult result)
    {
        OnGameFinished?.Invoke(result);
    }

    public void EndGame()
    {
        gameObject.SetActive(false);
    }
}
