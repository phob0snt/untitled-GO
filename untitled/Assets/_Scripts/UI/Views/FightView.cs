using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class FightView : View
{
    public UnityEvent OnPressAttack = new();
    public UnityEvent OnPressBarrier = new();

    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _barrierButton;

    public override void Init()
    {
    }

}
