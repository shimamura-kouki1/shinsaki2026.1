using UnityEngine;
using UnityEngine.InputSystem;

public class PasswordMinigame : BaseMinigame
{
    [SerializeField] private int _targetCount = 3;

    private int _currentCount;
    private Key _currentKey;


    private readonly Key[] keys =
    {Key.A,
    Key.S,
    Key.D,
    Key.W,
    Key.Space,
    Key.LeftShift, };

    private void Update()
    {
        if (Keyboard.current[_currentKey].wasPressedThisFrame)
        {
            _currentCount++;

            if(_currentCount >= _targetCount)
            {
                Finish(MinigameResult.Success);
            }
            else
            {
                NextKey();
            }
        }
    }

    public override void StartGame()
    {
        base.StartGame();

        _currentCount = 0;
        NextKey();
    }

    private void NextKey()
    {
        _currentKey = keys[Random.Range(0, keys.Length)];
        Debug.Log($"押すキー: {_currentKey}");
    }


}
