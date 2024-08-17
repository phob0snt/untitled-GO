using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : View
{
    [SerializeField] private Button _characterButton;
    [SerializeField] private Button _itemsUpgradeButton;
    [SerializeField] private Button _storageButton;

    [SerializeField] private List<Tab> _tabs;
    public CharacterCustomizeTab CharacterCustomizeTab => _tabs[0] as CharacterCustomizeTab;
    public override void Init()
    {
        foreach (var tab in _tabs)
        {
            tab.Initialize();
        }
    }

    private void OnEnable()
    {
        _characterButton.onClick.AddListener(() => ShowTab(0));
        _itemsUpgradeButton.onClick.AddListener(() => ShowTab(1));
        _storageButton.onClick.AddListener(() => ShowTab(2));
    }

    private void OnDisable()
    {
        _characterButton.onClick.RemoveListener(() => ShowTab(0));
        _itemsUpgradeButton.onClick.RemoveListener(() => ShowTab(1));
        _storageButton.onClick.RemoveListener(() => ShowTab(2));
    }

    private void ShowTab(int index)
    {
        for (int i = 0; i < _tabs.Count; i++)
        {
            if (i == index)
                _tabs[i].gameObject.SetActive(true);
            else
                _tabs[i].gameObject.SetActive(false);
        }
    }
}
