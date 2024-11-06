using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(IPlayerController player, Animator animator) : base(player, animator) { }
    public override void Enter()
    {
        int animationHash = 0;
        PlayerController controller = _playerController as PlayerController;
        switch(controller.AttackType)
        {
            case AttackType.Explosion:
                animationHash = FirstAttackHash;
                break;
            case AttackType.Heatwave:
                animationHash = SecondAttackHash;
                break;
        }
        if (animationHash == 0) return;
        _animator.CrossFadeInFixedTime(animationHash, CROSSFADE_DURATION);
    }

}
