using Zenject;
using UnityEngine;

public class DependenciesMainSceneInstaller : MonoInstaller
{
    [Inject] private readonly DataManager _dataManager;
    [SerializeField] private Character _character;
    [SerializeField] private PlayerLocationHandler _locationHandler;
    [SerializeField] private ItemUIPool _itemUIPool;
    [SerializeField] private LocalObjectsSpawner _localObjectsSpawner;
    public override void InstallBindings()
    {
        Container.Bind<Character>().FromInstance(_character).AsSingle().NonLazy();
        Container.Bind<ItemUIPool>().FromInstance(_itemUIPool).AsSingle().NonLazy();
        Container.Bind<LocalObjectsSpawner>().FromInstance(_localObjectsSpawner).AsSingle().NonLazy();
        Container.Bind<PlayerLocationHandler>().FromInstance(_locationHandler).AsSingle().NonLazy();
        _dataManager.ReInject(Container);
    }
}
