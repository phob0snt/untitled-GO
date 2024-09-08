using UnityEngine;

public class LocomotionState : BaseState
{
    public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime(LocomotionHash, CROSSFADE_DURATION);
        Debug.Log("Entered Loco");
    }

    public override void Update()
    {
        _playerController.Move();
    }
}
