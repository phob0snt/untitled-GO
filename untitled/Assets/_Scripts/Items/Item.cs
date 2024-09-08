using UnityEngine;

public abstract class Item : ScriptableObject
{
    public abstract ItemType Type { get; }
    public string Name => _name;
    [SerializeField] private string _name;
    public string Description => _description;
    [SerializeField] private string _description;
    public Sprite Icon => _icon;
    [SerializeField] private Sprite _icon;
    public virtual void Initialize(int level, int amount)
    {

    }
}

public enum ItemType
{
    Shoes,
    Pants,
    Outerwear,
    Ring,
    Hair,
    UpgradeComponent
}

public enum ClothSet
{
    None,
    Set1,
    Set2,
    Set3,
    Set4,
    Set5
}
