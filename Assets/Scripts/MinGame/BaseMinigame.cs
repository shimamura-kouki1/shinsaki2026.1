using UnityEngine;
using System;

public abstract class BaseMinigame : MonoBehaviour, IMinigame
{
    public event Action<MinigameResult> OnGameFinished;

    /// <summary>ゲームタイトル 各ミニゲームでoverride</summary>
    public virtual string Title => "No Title";

    /// <summary>ゲーム説明</summary>
    public virtual string Description => "";

    protected bool IsPlaying;

    public virtual void StartGame()
    {
        IsPlaying = true;
        Debug.Log("ミニゲーム開始！");
    }

    protected void Finish(MinigameResult result)
    {
        IsPlaying = false;
        OnGameFinished?.Invoke(result);
    }

    public void EndGame()
    {
        IsPlaying = false;
        OnEndGame();
    }
    protected virtual void OnEndGame() { }
}
