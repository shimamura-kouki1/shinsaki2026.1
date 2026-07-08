using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;

    [SerializeField] private TextMeshProUGUI _connectionRateText;
    [SerializeField] private TextMeshProUGUI _statusText;

    [SerializeField] private Image _warningPanel;

    /// <summary>
    /// UIを初期化
    /// </summary>
    public void ResetUI()
    {
        UpDateGauge(0f, 100f);
        SetStatus("接続中...");
        HideWarning();
    }

    /// <summary>
    /// ゲージ更新
    /// </summary>
    public void UpDateGauge(float current, float max)
    {
        float ratio = current / max;

        _slider.value = ratio;
        _connectionRateText.text = $"接続率{(int)current}%";

        UpdateGaugeColors(ratio);
    }

    /// <summary>
    /// ゲージ色更新
    /// </summary>
    public void UpdateGaugeColors(float ratio)
    {
        if(ratio < 0.3f)
        {
            _fillImage.color = Color.red;
        }
        else if (ratio < 0.7f)
        {
            _fillImage.color = Color.yellow;
        }
        else
        {
            _fillImage.color = Color.green;
        }
    }

    /// <summary>
    /// 状態表示
    /// </summary>
    public void SetStatus(string Status)
    {
        _statusText.text = Status;
    }

    /// <summary>
    /// 警告表示
    /// </summary>
    public void ShowWarning()
    {
        Color color = _warningPanel.color;
        color.a = 0.3f;
        _warningPanel.color = color;
    }

    /// <summary>
    /// 警告非表示
    /// </summary>
    public void HideWarning()
    {
        Color color = _warningPanel.color;
        color.a = 0f;
        _warningPanel.color = color;
    }
}