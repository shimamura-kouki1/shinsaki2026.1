using TMPro;
using UnityEngine;
using System.Collections;
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
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        _countdownPanel.SetActive(false);
        _instructionPanel.SetActive(false);
        _resultPanel.SetActive(false);
    }


    /// <summary>
    /// ラウンドを表示
    /// </summary>
    /// <param name="round">ラウンドの</param>
    public void UpdateRound(int round)
    {
        _roundText.text = $"Round{round}";
    }

    /// <summary>
    /// タイムUI
    /// </summary>
    /// <param name="time"></param>
    public void UpdateTimeUI(float time)
    {
        _timeText.text = time.ToString("F1");
    }

    public void UpdateLife(int life)
    {
        _lifeText.text = $"Life : {life}";
    }

    /// <summary>
    /// カウントダウン処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            ShowCountdown(i.ToString());
            yield return new WaitForSeconds(1f);
        }

        ShowCountdown("GO");
        yield return new WaitForSeconds(0.5f);

        HideCountdown();
    }

    /// <summary>
    /// カウントダウンの表示
    /// </summary>
    /// <param name="text"></param>
    public void ShowCountdown(string text)
    {
        _countdownPanel.SetActive(true);

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
        _instructionPanel.SetActive(true);

        _titleText.text = title;
        _descriptionText.text = description;
    }

    /// <summary>
    /// ミニゲームの非表示
    /// </summary>
    public void HideInstruction()
    {
        _instructionPanel.SetActive(false);
    }

    /// <summary>
    /// リザルト画面の表示
    /// </summary>
    /// <param name="result"></param>
    public void ShowResult(MinigameResult result)
    {
        _resultPanel.SetActive(true);

        _resultText.text = result == MinigameResult.Success ? _successText : _failText;
    }

    /// <summary>
    /// リザルト画面の非表示
    /// </summary>
    public void HideResult()
    {
        _resultPanel.SetActive(false);
    }
}
