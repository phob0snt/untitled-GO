using Zenject;
using UnityEngine;

public class DependenciesMainSceneInstaller : MonoInstaller
{
    [SerializeField] private Character _character;
    public override void InstallBindings()
    {
        Container.Bind<Character>().FromInstance(_character).AsSingle().NonLazy();
        //Container.Bind<IEnemyFactory>().To<EnemyFactory>().FromInstance(_enemyFactory).AsSingle();
    }
}
