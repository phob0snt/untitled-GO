using UnityEngine;
using Zenject;

public class ManagersMainSceneInstaller : MonoInstaller
{
    [SerializeField] private ViewManager _viewManager;
    [SerializeField] private ProgressManager _progressManager;

    public override void InstallBindings()
    {
        Container.Bind<ViewManager>().FromInstance(_viewManager).AsSingle().NonLazy();
        Container.Bind<ProgressManager>().FromInstance(_progressManager).AsSingle().NonLazy();
    }
}
