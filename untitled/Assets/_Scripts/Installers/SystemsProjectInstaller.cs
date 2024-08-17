using UnityEngine;
using Zenject;

public class SystemsProjectInstaller : MonoInstaller
{
    [SerializeField] private GameObject _systemsPrefab;

    public override void InstallBindings()
    {
        Container.InstantiatePrefab(_systemsPrefab);
        GameManager gameManager = GetComponentInChildren<GameManager>();
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
    }
}
