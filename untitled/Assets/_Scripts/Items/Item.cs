using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemData")]
public class Item : ScriptableObject
{
    public ItemType Type => _type;
    [SerializeField] private ItemType _type;
    public string Name => _name;
    [SerializeField] private string _name;
    public string Description => _description;
    [SerializeField] private string _description;
    public Sprite Icon => _icon;
    [SerializeField] private Sprite _icon;
}

public enum ItemType
{
    Shoes,
    Pants,
    Outerwear,
    Ring,
    Other
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
