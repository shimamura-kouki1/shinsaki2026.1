using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CableDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public event Action OnConnected;

    [Header("接続点")]
    [SerializeField] private CablePoint _startPoint;
    [SerializeField] private CablePoint _endPoint;

    [Header("ケーブル画像")]
    [SerializeField] private RectTransform _cableImage;

    [Header("判定距離")]
    [SerializeField] private float _connectDistance = 35f;

    /// <summary>接続済みか</summary>
    public bool IsConnected { get; private set; }

    private bool _isDragging;
    private RectTransform _canvasRect;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();

        if (_canvas == null)
        {
            Debug.LogError($"{nameof(CableDrag)} requires a parent Canvas.", this);
            return;
        }

        _canvasRect = _canvas.GetComponent<RectTransform>();

        ResetCable();
    }

    public void ResetCable()
    {
        IsConnected = false;
        _isDragging = false;

        _cableImage.gameObject.SetActive(false);

        _cableImage.sizeDelta = new Vector2(0f, _cableImage.sizeDelta.y);

        _cableImage.rotation = Quaternion.identity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsConnected) return;

        _isDragging = true;

        _cableImage.gameObject.SetActive(true);
        UpdateCable(GetMousePosition());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;

        UpdateCable(GetMousePosition());
    }

    /// <summary>
    /// ドラッグ終了
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isDragging) return;

        _isDragging = false;

        if (CanConnect())
        {
            Connect();
        }
        else
        {
            ResetCable();
        }

    }

    /// <summary>
    /// 接続可能かの判定
    /// </summary>
    /// <returns></returns>
    private bool CanConnect()
    {
        if (_startPoint.Color != _endPoint.Color) return false;

        Vector2 mouse = GetMousePosition();

        RectTransform endRect = (RectTransform)_endPoint.transform;

        float distance = Vector2.Distance(mouse, endRect.position);

        return distance <= _connectDistance;
    }

    /// <summary>
    /// 接続成功時の処理
    /// </summary>
    private void Connect()
    {
        IsConnected = true;

        UpdateCable(((RectTransform)_endPoint.transform).position);

        OnConnected?.Invoke();
    }

    /// <summary>
    /// ケーブルの更新
    /// </summary>
    /// <param name="endPosition"></param>
    private void UpdateCable(Vector2 endPosition)
    {
        Vector2 direction = endPosition - (Vector2)_startPoint.transform.position;

        float length = direction.magnitude;

        RectTransform start = (RectTransform)_startPoint.transform;

        _cableImage.position = start.position;

        _cableImage.sizeDelta =
           new Vector2(length, _cableImage.sizeDelta.y);

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _cableImage.rotation =
            Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// マウスの座標
    /// </summary>
    /// <returns></returns>
    private Vector2 GetMousePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
           _canvasRect,
           Mouse.current.position.ReadValue(),
           _canvas.worldCamera,
           out Vector2 localPoint);
        return _canvasRect.TransformPoint(localPoint);
    }
}
