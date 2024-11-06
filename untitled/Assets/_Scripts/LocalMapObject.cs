using System;
using UnityEngine;

public enum MapObjectType
{
    Battle,

}

public enum MapObjectRareness
{
    Common,
    Uncommon,
    Rare,
    VeryRare,
    Special
}

public abstract class LocalMapObject : ScriptableObject
{
    protected MapObjectRareness _rareness;
    public virtual void Initialize()
    {

    }

    public virtual void RandomConfiguration()
    {
        _rareness = (MapObjectRareness)UnityEngine.Random.Range(0, Enum.GetNames(typeof(MapObjectRareness)).Length);
    }
}
