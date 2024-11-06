using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(IPlayerController player, Animator animator) : base(player, animator) { }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime(IdleHash, CROSSFADE_DURATION / 3);
    }
}
