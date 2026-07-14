using UnityEngine;

public class CableGame : BaseMinigame
{
    [Header("ケーブルの一覧")]
    [SerializeField] private CableDrag[] _cables;

    [SerializeField] private RectTransform[] _endPoints;
    [SerializeField] private GameObject _CableGameUI;

    [SerializeField] private string _titleText = "線でつなげ";
    [SerializeField] private string _descriptionText = "ドラッグして同じ色をつなげろ";

    public override string Title => _titleText;

    public override string Description => _descriptionText;

    public override void StartGame()
    {
        base.StartGame();

        _CableGameUI.SetActive(true);

        ShuffleEndPoints();
        foreach (CableDrag cableDrag in _cables)
        {
            cableDrag.ResetCable();
            cableDrag.OnConnected -= CheckClear;
            cableDrag.OnConnected += CheckClear;
        }
    }

    protected override void OnEndGame()
    {
        _CableGameUI.SetActive(false);
        foreach (CableDrag cableDrag in _cables)
        {
            cableDrag.OnConnected -= CheckClear;
        }
    }

    private void CheckClear()
    {
        foreach (CableDrag cable in _cables)
        {
            if (!cable.IsConnected)
            {
                return;
            }
        }

        Debug.Log("ケーブル接続完了！");
        Finish(MinigameResult.Success);
    }

    private void ShuffleEndPoints()
    {
        for (int i = 0; i < _endPoints.Length; i++)
        {
            int random = Random.Range(i, _endPoints.Length);

            Vector3 pos = _endPoints[i].position;

            _endPoints[i].position = _endPoints[random].position;
            _endPoints[random].position = pos;
        }
    }
}
