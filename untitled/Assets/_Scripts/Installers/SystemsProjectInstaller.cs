using DG.Tweening.Core.Easing;
using UnityEngine;
using Zenject;

public class SystemsProjectInstaller : MonoInstaller
{
    [SerializeField] private GameObject _systemsPrefab;

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromComponentInHierarchy(_systemsPrefab).AsSingle().NonLazy();
        Container.Bind<DataManager>().FromComponentInHierarchy(_systemsPrefab).AsSingle().NonLazy();
        Container.InstantiatePrefab(_systemsPrefab);
    }
}
