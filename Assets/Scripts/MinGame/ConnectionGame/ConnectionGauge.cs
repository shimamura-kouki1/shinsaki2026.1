using UnityEngine;

public class ConnectionGauge : MonoBehaviour
{
    [SerializeField] private float _disconnectLimit = 3f;

    [SerializeField,Tooltip("一秒の増加割合")] private float _increaseSpeed = 20f;

    [SerializeField,Tooltip("一秒の減少割合")] private float _recoverSpeed = 15f;

    private float _currentTime;

    public float MaxValue => _disconnectLimit;
    public float Ratio => _currentTime / _disconnectLimit;

    public bool IsDisconnected => _currentTime >= _disconnectLimit;

    /// <summary>現在の接続率</summary>
    public float CurrentValue => _currentTime;

    /// <summary>ゲージが最大か</summary>
    public bool IsFull => _currentTime >= _disconnectLimit;

    /// <summary>
    /// 接続率を減少
    /// </summary>
    public void Recover(float deltaTime)
    {
        _currentTime -= _recoverSpeed * deltaTime;
        _currentTime = Mathf.Clamp(_currentTime, 0, _disconnectLimit);
    }

    /// <summary>
    /// 接続率の増加
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Increase(float deltaTime)
    {
        _currentTime += _increaseSpeed * deltaTime;
        _currentTime = Mathf.Clamp(_currentTime, 0, _disconnectLimit);
    }

    /// <summary>
    /// ゲージの初期化
    /// </summary>
    public void ResetGauge()
    {
        _currentTime = 0f;
    }
}
