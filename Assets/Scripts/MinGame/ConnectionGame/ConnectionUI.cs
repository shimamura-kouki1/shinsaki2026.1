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


    public void ResetUI()
    {

    }

    public void UpDateGauge(float current, float max)
    {
        float ratio = current / max;

        _slider.value = ratio;
        _connectionRateText.text = $"接続率{(int)current}%";
    }

    private void GaugeColors(float ratio)
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
}