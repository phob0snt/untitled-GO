using UnityEngine;
using Zenject;

public class LocalBattle : LocalMapObject
{
    private BattleData _battleData;
    public override void RandomConfiguration()
    {
        base.RandomConfiguration();
        _battleData = new();
        switch (_rareness)
        {
            case MapObjectRareness.Common:
                _battleData.Reward = new Reward();
                break;
        }
    }
}
