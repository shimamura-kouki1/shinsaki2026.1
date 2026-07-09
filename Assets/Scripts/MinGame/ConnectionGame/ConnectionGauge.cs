using UnityEngine;

public class ConnectionGauge : MonoBehaviour
{
    [SerializeField] private float _disconnectLimit = 3f;

    [SerializeField,Tooltip("一秒の増加割合")] private float _increaseSpeed = 1f;

    [SerializeField,Tooltip("一秒の減少割合")] private float _recoverSpeed = 2f;

    private float _currentTime;

    public float DisconnectLimit => _disconnectLimit;
    public float Ratio => _currentTime / _disconnectLimit;

    public bool IsDisconnected => _currentTime >= _disconnectLimit;

    /// <summary>現在の接続率</summary>
    public float CurrentTime => _currentTime;

    /// <summary>
    /// 切断ゲージを回復
    /// </summary>
    public void Recover(float deltaTime)
    {
        _currentTime -= _recoverSpeed * deltaTime;
        _currentTime = Mathf.Clamp(_currentTime, 0, _disconnectLimit);
    }

    /// <summary>
    ///　切断ゲージを増加
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
