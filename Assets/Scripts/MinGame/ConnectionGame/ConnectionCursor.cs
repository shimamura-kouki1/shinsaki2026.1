using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionCursor : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveRange = 300f;

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
    }
}
