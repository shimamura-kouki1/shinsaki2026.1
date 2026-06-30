using UnityEngine;
using UnityEngine.InputSystem;

public class testMinGame : BaseMinigame
{
    [SerializeField] private int _clearCount = 5;
    private int _currentCount = 0;

    public override void StartGame()
    {
        _currentCount = 0;

        Debug.Log("ミニゲーム開始！エンターキー連打しろ！");

        IsPlaying = true;
    }

    void Update()
    {
        if (!IsPlaying) return;
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            _currentCount++;
            Debug.Log("エンターキーを押しました！現在の回数: " + _currentCount);

            if (_currentCount >= _clearCount)
            {
                Debug.Log("ミニゲームクリア！おめでとう！");
                Finish(MinigameResult.Success);
            }
        }
    }
}
