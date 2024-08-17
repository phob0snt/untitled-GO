using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainView : MonoBehaviour
{
    [Inject] private readonly ViewManager _viewManager;
    [SerializeField] private Button _mapButton;
    [SerializeField] private Button _purchasesButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _squadsButton;
    [SerializeField] private Button _profileButton;

    private void OnEnable()
    {
        _mapButton.onClick.AddListener(() => _viewManager.Show<MapView>(false));
        _purchasesButton.onClick.AddListener(() => _viewManager.Show<InAppPurchasesView>(false));
        _inventoryButton.onClick.AddListener(() => _viewManager.Show<InventoryView>(false));
        _squadsButton.onClick.AddListener(() => _viewManager.Show<SquadsView>(false));
        _profileButton.onClick.AddListener(() => _viewManager.Show<ProfileView>(false));
    }

    private void OnDisable()
    {
        _mapButton.onClick.RemoveListener(() => _viewManager.Show<MapView>(false));
        _purchasesButton.onClick.RemoveListener(() => _viewManager.Show<InAppPurchasesView>(false));
        _inventoryButton.onClick.RemoveListener(() => _viewManager.Show<InventoryView>(false));
        _squadsButton.onClick.RemoveListener(() => _viewManager.Show<SquadsView>(false));
        _profileButton.onClick.RemoveListener(() => _viewManager.Show<ProfileView>(false));
    }
}
