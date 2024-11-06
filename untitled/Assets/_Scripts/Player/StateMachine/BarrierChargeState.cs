using UnityEngine;

public class BarrierChargeState : BaseState
{
    public BarrierChargeState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime(BarrierChargeHash, CROSSFADE_DURATION);
        Debug.Log("Entered Barrier");
    }
}
