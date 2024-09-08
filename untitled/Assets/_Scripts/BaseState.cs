using UnityEngine;

public abstract class BaseState : IState
{
    protected readonly PlayerController _playerController;
    protected readonly Animator _animator;

    protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    protected static readonly int BarrierChargeHash = Animator.StringToHash("BarrierCharge");


    protected const float CROSSFADE_DURATION = 0.4f;

    protected BaseState(PlayerController player, Animator animator)
    {
        _playerController = player;
        _animator = animator;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
        Debug.Log("Exit state");
    }
}
