using UnityEngine;

public class ConnectionGauge : MonoBehaviour
{
    [SerializeField] private float _maxValue = 100f;

    [SerializeField,Tooltip("一秒の増加割合")] private float _increaseSpeed = 20f;

    [SerializeField,Tooltip("一秒の減少割合")] private float _decreaseSpeed = 15f;

    private float _currentValue;

    /// <summary>現在の接続率</summary>
    public float CurrentValue => _currentValue;

    /// <summary>ゲージが最大か</summary>
    public bool IsFull => _currentValue >= _maxValue;

    /// <summary>
    /// 接続率を減少
    /// </summary>
    public void Decrease(float deltaTime)
    {
        _currentValue -= _decreaseSpeed * deltaTime;
        _currentValue = Mathf.Clamp(_currentValue, 0, _maxValue);
    }

    /// <summary>
    /// 接続率の増加
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Increase(float deltaTime)
    {
        _currentValue += _increaseSpeed * deltaTime;
        _currentValue = Mathf.Clamp(_currentValue, 0, _maxValue);
    }

    /// <summary>
    /// ゲージの初期化
    /// </summary>
    public void ResetGauge()
    {
        _currentValue = 0f;
    }
}
