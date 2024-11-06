using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapView : View
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly Character _char;

    [SerializeField] private Button _fightButton;
    [SerializeField] private BattleData _battleData;
    

    private void OnEnable()
    {
        _fightButton.onClick.AddListener(() => _gameManager.LoadFightScene(_battleData));
    }

    private void OnDisable()
    {
        _fightButton.onClick.RemoveListener(() => _gameManager.LoadFightScene(_battleData));
    }
}
