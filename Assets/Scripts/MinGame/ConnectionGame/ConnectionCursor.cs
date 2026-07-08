using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionCursor : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveRange = 300f;

    private RectTransform _rectTransform;
    private float _positionX;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null) return;

        if (keyboard.aKey.wasPressedThisFrame)
        {

        }
    }
}
