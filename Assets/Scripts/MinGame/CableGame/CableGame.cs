using UnityEngine;

public class CableGame : BaseMinigame
{
    [Header("ケーブルの一覧")]
    [SerializeField] private CableDrag[] _cables;

    public override void StartGame()
    {
        base.StartGame();

        foreach(CableDrag cableDrag in _cables)
        {
            cableDrag.ResetCable();
            cableDrag.OnConnected += CheckClear;
        }
    }

    protected override void OnEndGame()
    {
        foreach(CableDrag cableDrag in _cables)
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
}
