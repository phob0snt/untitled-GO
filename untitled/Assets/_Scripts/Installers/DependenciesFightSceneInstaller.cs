using UnityEngine;
using Zenject;

public class DependenciesFightSceneInstaller : MonoInstaller
{
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private Player _player;
    [SerializeField] private InputHandler _inputHandler;

    public override void InstallBindings()
    {
        Container.Bind<IEnemyFactory>().To<EnemyFactory>().FromInstance(_enemyFactory).AsSingle();
        Container.Bind<Player>().FromInstance(_player).AsSingle();
        Container.Bind<InputHandler>().FromInstance(_inputHandler).AsSingle();
    }
}
