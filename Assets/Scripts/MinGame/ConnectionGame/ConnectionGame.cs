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
    [SerializeField] private float _warningRate = 0.8f;

    [SerializeField] private string _title;
    [SerializeField] private string _description;

    [SerializeField] private float _sEVolume = 0.3f;

    public override string Title => _title;
    public override string Description => _description;

    private float _timer;

    private bool _warningPlayed;

    public override void StartGame()
    {
        base.StartGame();

        _timer = 0f;
        _warningPlayed = false;
        _cursor.ResetPosition();
        _gauge.ResetGauge();
        _ui.ResetUI(_gauge.DisconnectLimit);
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
            if (!_warningPlayed)
            {
                _warningPlayed = true;
                AudioManager.Instance.PlaySE(SENames.Warning,_sEVolume);
            }
            _gauge.Increase(Time.deltaTime);
            _ui.SetStatus("接続中...");
        }

        //---------------------------------
        // UI更新
        //---------------------------------
        _ui.UpDateGauge(_gauge.CurrentTime, _gauge.DisconnectLimit);

        //---------------------------------
        // 警告
        //---------------------------------
        if (_gauge.Ratio >= _warningRate)
        {
            _ui.ShowWarning();

           
        }
        else
        {
            _ui.HideWarning();
            _warningPlayed = false;
        }

        // 失敗判定
        if (_gauge.IsDisconnected)
        {
            Debug.Log("通信切断");
            _ui.SetStatus("通信切断");
            Finish(MinigameResult.Failure);
        }
    }

    private bool IsInSafeZone()
    {
        float cursorX = _cursor.PositionX;
        float safeCenter = _safeZone.anchoredPosition.x;

        float halfWidth = _safeZone.rect.width * 0.5f;

        return Mathf.Abs(cursorX - safeCenter) <= halfWidth;
    }

    protected override void OnEndGame()
    {
        _ui.HideWarning();
    }
}
