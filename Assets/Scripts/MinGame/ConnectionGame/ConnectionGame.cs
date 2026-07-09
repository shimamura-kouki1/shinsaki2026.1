using UnityEngine;

public class ConnectionGame : BaseMinigame
{

    [Header("参照")]
    [SerializeField] private ConnectionCursor _cursor;
    [SerializeField] private ConnectionGauge _gauge;
    [SerializeField] private ConnectionUI _ui;

    [Header("判定")]
    [SerializeField] private RectTransform _safeZone;
    [SerializeField] private float _safeRange = 60f;

    [SerializeField,Tooltip("BaseMinigameのTimeLimitを超えないように注意")] private float _surviveTime = 5f;

    private float _timer;

    private void Start()
    {
        StartGame();
    }

    public override void StartGame()
    {
        base.StartGame();

        _timer = 0f;
        _cursor.ResetPosition();
        _gauge.ResetGauge();
        _ui.ResetUI();
    }

    private void Update()
    {
        if (!IsPlaying) return;

        _timer += Time.deltaTime;

        if (_timer >= _surviveTime)
        {
            _ui.SetStatus("同期完了！");
            Finish(MinigameResult.Success);
            return;
        }

        //---------------------------------
        // カーソル移動
        //---------------------------------
        _cursor.Move();

        //---------------------------------
        // 安定ゾーン判定
        //---------------------------------
        bool isSafe = IsInSafeZone();

        if (isSafe)
        {
            _gauge.Recover(Time.deltaTime);
            _ui.SetStatus("通信安定");
        }
        else
        {
            _gauge.Increase(Time.deltaTime);
            _ui.SetStatus("接続中...");
        }

        //---------------------------------
        // UI更新
        //---------------------------------
        _ui.UpDateGauge(_gauge.CurrentValue, _gauge.MaxValue);

        //---------------------------------
        // 警告
        //---------------------------------
        if (_gauge.CurrentValue <= 20)
        {
            _ui.ShowWarning();
        }
        else
        {
            _ui.HideWarning();
        }

        //---------------------------------
        // クリア判定
        //---------------------------------
        if (_gauge.IsDisconnected)
        {
            Debug.Log("クリア");
            _ui.SetStatus("通信切断");
            Finish(MinigameResult.Failure);
        }
    }

    private bool IsInSafeZone()
    {
        float cursorX = _cursor._positionX;
        float safeCenter = _safeZone.anchoredPosition.x;

        return Mathf.Abs(cursorX - safeCenter) <= _safeRange;
    }

    protected override void OnEndGame()
    {
        _ui.HideWarning();
    }
}
