using System;
using System.Collections.Generic;

[Serializable]
public class Reward
{
    public int CoinsAmount { get; private set; }
    public int XPAmount { get; private set; }
    public List<InventoryItem<Item>> Items { get; private set; }

    private readonly Dictionary<MapObjectRareness, Range> _coinsRange = new()
    {
        { MapObjectRareness.Common, new Range(30, 55) },
        { MapObjectRareness.Uncommon, new Range(60, 95) },
        { MapObjectRareness.Rare, new Range(110, 180) },
        { MapObjectRareness.VeryRare, new Range(195, 305) },
        { MapObjectRareness.Special, new Range(320, 510) }
    };

    private readonly Dictionary<MapObjectRareness, Range> _XPRange = new()
    {
        { MapObjectRareness.Common, new Range(14, 26) },
        { MapObjectRareness.Uncommon, new Range(34, 58) },
        { MapObjectRareness.Rare, new Range(61, 99) },
        { MapObjectRareness.VeryRare, new Range(142, 230) },
        { MapObjectRareness.Special, new Range(284, 453) }
    };


    public void RandomConfigure(MapObjectRareness rareness)
    {
        switch (rareness)
        {
            case MapObjectRareness.Common:
                XPAmount = UnityEngine.Random.Range(_XPRange[rareness].Start.Value, _XPRange[rareness].End.Value);
                //XPAmount *= 
                break;
        }
    }
}
