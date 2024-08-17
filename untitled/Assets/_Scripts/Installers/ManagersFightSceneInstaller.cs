using UnityEngine;
using Zenject;

public class ManagersFightSceneInstaller : MonoInstaller
{
    [SerializeField] private ViewManager _viewManager;
    [SerializeField] private FightManager _fightManager;

    public override void InstallBindings()
    {
        Container.Bind<ViewManager>().FromInstance(_viewManager).AsSingle();
        Container.Bind<FightManager>().FromInstance(_fightManager).AsSingle();
    }
}
