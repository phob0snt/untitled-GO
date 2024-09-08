using System.Collections.Generic;
using UnityEngine;

public class ItemUIPool : MonoBehaviour
{
    [SerializeField] private ItemUI _itemUIPrefab;

    private Queue<ItemUI> _itemUIPool = new();

    public ItemUI GetItemUI(Transform parent)
    {
        if (_itemUIPool.Count > 0)
        {
            var itemUI = _itemUIPool.Dequeue();
            itemUI.gameObject.SetActive(true);
            itemUI.transform.SetParent(parent);
            return itemUI;
        }

        return Instantiate(_itemUIPrefab, parent);
    }

    public void ReturnItemUI(ItemUI itemUI)
    {
        itemUI.gameObject.SetActive(false);
        _itemUIPool.Enqueue(itemUI);
    }

    public void ClearAllItems(Transform parent)
    {
        Debug.Log("cleared");
        foreach (Transform child in parent)
        {
            var itemUI = child.GetComponent<ItemUI>();
            if (itemUI != null)
                ReturnItemUI(itemUI);
        }
    }
}
