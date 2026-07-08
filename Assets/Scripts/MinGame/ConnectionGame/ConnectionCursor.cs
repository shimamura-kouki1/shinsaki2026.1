using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionCursor : MonoBehaviour
{
    [Header("プレイヤー移動")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("ランダム移動")]
    [SerializeField] private float _changeMinTime = 1.5f;
    [SerializeField] private float _changeMaxTime = 3f;

    [Header("自動移動")]
    [SerializeField] private float _autoSpeed = 150f;
    [SerializeField] private float _moveRange = 300f;

    private float _autoDirection = 1f;

    private float _changeTimer;
    private float _nextChangeTime;

    private RectTransform _rectTransform;

    public float _positionX => _rectTransform.anchoredPosition.x;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// カーソルの移動
    /// </summary>
    public void Move()
    {
        ChangeDirection();
        float inputVector = 0f;

        Keyboard keyboard = Keyboard.current;

        if(keyboard != null)
        {
            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
            {
                inputVector = -1f;
            }

            if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
            {
                inputVector = 1f;
            }
        }

        Vector2 pos = _rectTransform.anchoredPosition;

        //自動移動
        pos.x += _autoSpeed * _autoDirection * Time.deltaTime;
        //プレイヤー移動
        pos.x += inputVector * _moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x,-_moveRange,_moveRange);

        _rectTransform.anchoredPosition = pos;
    }

    /// <summary>
    /// 中央に戻す
    /// </summary>
    public void ResetPosition()
    {
        _rectTransform.anchoredPosition = Vector2.zero;

        _changeTimer = 0f;
        _nextChangeTime = Random.Range(_changeMinTime, _changeMaxTime);

        _autoDirection = Random.value < 0.5f ? -1f : 1f;
    }

    /// <summary>
    /// 一定時間ごとにランダムで方向変更
    /// </summary>
    private void ChangeDirection()
    {
        _changeTimer += Time.deltaTime;

        if (_changeTimer >= _nextChangeTime)
        {
            _changeTimer = 0f;

            _nextChangeTime = Random.Range(_changeMinTime, _changeMaxTime);

            _autoDirection = Random.value < 0.5f ? -1f : 1f;
        }
    }
}
