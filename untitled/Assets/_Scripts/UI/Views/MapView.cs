using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MapView : View
{
    [Inject] private GameManager _gameManager;
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

    public override void Init()
    {
    }
}
