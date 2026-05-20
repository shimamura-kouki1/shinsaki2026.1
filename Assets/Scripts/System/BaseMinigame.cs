using UnityEngine;
using System;

public abstract class BaseMinigame : MonoBehaviour, IMinigame
{
    public event Action<MinigameResult> OnGameFinished;

    protected bool IsPlaying;

    public virtual void StartGame()
    {
        IsPlaying = true;
    }

    protected void Finish(MinigameResult result)
    {
        IsPlaying = false;
        OnGameFinished?.Invoke(result);
    }

    public void EndGame()
    {
        IsPlaying = false;
        gameObject.SetActive(false);
    }
}
