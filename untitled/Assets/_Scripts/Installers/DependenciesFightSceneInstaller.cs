using UnityEngine;
using Zenject;

public class DependenciesFightSceneInstaller : MonoInstaller
{
    [SerializeField] private EnemyFactory _enemyFactory;

    public override void InstallBindings()
    {
        Container.Bind<IEnemyFactory>().To<EnemyFactory>().FromInstance(_enemyFactory).AsSingle();
    }
}
