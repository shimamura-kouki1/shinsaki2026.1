using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PasswordMinigame : BaseMinigame
{
    [SerializeField, Min(1)] private int _passwordLength = 3;
    [SerializeField] private PasswordGameUI _gameUI;

    public override string Title => "パスワードを打ち込め！";

    public override string Description => "表示されたキーを順番に入力しろ";

    private int _currentIndex;
    private readonly List<Key> _password = new();

    private readonly Key[] _keys =
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F,
        Key.G, Key.H, Key.I, Key.J, Key.K, Key.L,
        Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R,
        Key.S, Key.T, Key.U, Key.V, Key.W, Key.X,
        Key.Y, Key.Z,
    };



    private void Update()
    {
        if (!IsPlaying) return;

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)return;

        if (!keyboard.anyKey.wasPressedThisFrame)
            return;

        if (_currentIndex >= _password.Count)
            return;

        if (keyboard[_password[_currentIndex]].wasPressedThisFrame)
        {
            _currentIndex++;

            UpdatePasswordText();

            if (_currentIndex >= _password.Count)
            {
                Finish(MinigameResult.Success);
            }
        }
        else
        {
            Finish(MinigameResult.Failure);
        }
    }

    public override void StartGame()
    {
        base.StartGame();

        _currentIndex = 0;

        _gameUI.Show();
        CreatePassword();
        UpdatePasswordText();
    }

    protected override void OnEndGame()
    {
        _gameUI.Hide();
    }


    private void CreatePassword()
    {
        _password.Clear();

        for (int i = 0; i < _passwordLength; i++)
        {
            _password.Add(_keys[Random.Range(0, _keys.Length)]);
        }
    }

    private void UpdatePasswordText()
    {
        string text = "";

        for (int i = 0; i < _password.Count; i++)
        {
            if (i < _currentIndex)
            {
                text += "● ";
            }
            else
            {
                text += $"{_password[i]} ";
            }
        }
        _gameUI.UpdatePassword(text);
    }
}
