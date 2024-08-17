using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    public UnityEvent OnClick => GetComponent<Button>().onClick;

    public void SetItem(Item item)
    {
        _itemImage.sprite = item.Icon;
    }
}
