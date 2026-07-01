using TMPro;
using UnityEngine;
/// <summary>
/// UIの表示と更新を行うクラス
/// </summary>
public class GameUIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _roundText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _lifeText;

    [Header("CountDown")]
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _countdownText;

    [Header("Instruction")]
    [SerializeField] private GameObject _instructionPanel;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("Result")]
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private string _successText = "よくやった！";
    [SerializeField] private string _failText = "初めからやり直しこいや！";

    /// <summary>
    /// ラウンドを表示
    /// </summary>
    /// <param name="round">ラウンドの</param>
    public void UpDateRound(int round)
    {
        _roundText.text = $"Round{round}";
    }

    /// <summary>
    /// タイムUI
    /// </summary>
    /// <param name="time"></param>
    public void UpDateTimeUI(float time)
    {
        _timeText.text = time.ToString("F1");
    }

    public void UpdateLife(int life)
    {
        _lifeText.text = $"Life : {life}";
    }

    /// <summary>
    /// カウントダウンの表示
    /// </summary>
    /// <param name="text"></param>
    public void ShowCountdown(string text)
    {
        if (!_countdownPanel)
        {
            _countdownPanel.SetActive(true);
        }

        _countdownText.text = text;
    }

    /// <summary>
    /// カウントダウンの非表示
    /// </summary>
    public void HideCountdown()
    {
        _countdownPanel.SetActive(false);
    }

    /// <summary>
    /// ミニゲームの説明表示
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    public void ShowInstruction(string title, string description)
    {
        if (!_instructionPanel)
            _instructionPanel.SetActive(true);

        _timeText.text = title;
        _descriptionText.text = description;
    }

    /// <summary>
    /// ミニゲームの非表示
    /// </summary>
    public void HideInstruction()
    {
        _instructionPanel.SetActive(false);
    }

    public void ShowResult(bool success)
    {
        if (!_resultPanel)
            _resultPanel.SetActive(true);

        _resultText.text = success ? _successText : _failText;
    }
}
