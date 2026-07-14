using TMPro;
using UnityEngine;
using System.Collections;
using DG.Tweening;

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
        _roundText.text = $"Round : {round}";
    }

    /// <summary>
    /// タイムUI
    /// </summary>
    /// <param name="time"></param>
    public void UpdateTimeUI(float time)
    {
        _timeText.text = $"Time : {time.ToString("F1")}"; //time.ToString("F1");

        if (time <= 1f)
        {
            _timeText.color = Color.red;
        }
        else
        {
            _timeText.color = Color.white;
        }
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

        ShowCountdown("GO!");

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

        _countdownText.transform.DOKill();
        _countdownText.transform.localScale = Vector3.zero;
        if (text == "GO!")
        {
            _countdownText.transform
                .DOScale(2f, 0.35f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _countdownText.transform.DOScale(1f, 0.1f);
                });

        }
        else
        {
            _countdownText.transform.DOScale(1.3f, 0.25f).SetEase(Ease.OutBack);
        }
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

        _instructionPanel.transform.DOKill();

        _instructionPanel.transform.localScale = Vector3.zero;

        _instructionPanel.transform
            .DOScale(1f, 0.25f)
            .SetEase(Ease.OutBack);
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
        _resultText.color = result == MinigameResult.Success ? Color.green: Color.red;

        _resultPanel.transform.DOKill();
        _resultPanel.transform.localScale = Vector3.zero;

        _resultPanel.transform
            .DOScale(1.2f, 0.2f)
            .SetEase(Ease.OutBack);
    }

    /// <summary>
    /// リザルト画面の非表示
    /// </summary>
    public void HideResult()
    {
        _resultPanel.SetActive(false);
    }
}
