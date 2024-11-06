using UnityEngine;
using Zenject;

public class ManagersMainSceneInstaller : MonoInstaller
{
    [SerializeField] private ViewManager _viewManager;
    [SerializeField] private MapManager _mapManager;

    public override void InstallBindings()
    {
        Container.Bind<ViewManager>().FromInstance(_viewManager).AsSingle().NonLazy();
        Container.Bind<MapManager>().FromInstance(_mapManager).AsSingle().NonLazy();
    }
}
