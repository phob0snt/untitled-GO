using UnityEngine;
using DG.Tweening;
using Zenject;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController), typeof(Player))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [Inject] private readonly InputHandler _inputHandler;
    public AttackType AttackType => AttackType.Explosion;

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _rightHandRig;
    [SerializeField] private List<GameObject> _attackEffects;
    [SerializeField] private float _smoothRate;
    [SerializeField] private float _moveSpeed = 6f;

    private Vector2 _joystickInput => _inputHandler.GetJoystickInput();
    private Vector3 _moveDirection;

    private GameObject _currEffect;
    private bool _isSettingBarrier;
    private bool _isAttacking;
    private bool _isUltimating;

    private CharacterController _controller;
    private Player _player;
    private PlayerStateMachine _stateMachine;

    private readonly int _speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _player = GetComponent<Player>();

        _stateMachine = new PlayerStateMachine();

        var locomotionState = new LocomotionState(this, _animator);
        var barrierChargeState = new BarrierChargeState(this, _animator);
        var attackState = new AttackState(this, _animator);
        var idleState = new IdleState(this, _animator);

        _currEffect = Instantiate(_attackEffects[(int)AttackType], _rightHandRig);

        _stateMachine.AddTransition(idleState, locomotionState, new FuncPredicate(() => _animator.GetFloat(_speed) > 0));
        _stateMachine.AddTransition(locomotionState, idleState, new FuncPredicate(() => _animator.GetFloat(_speed) == 0));
        _stateMachine.AddTransition(idleState, barrierChargeState, new FuncPredicate(() => _isSettingBarrier));
        _stateMachine.AddTransition(locomotionState, barrierChargeState, new FuncPredicate(() => _isSettingBarrier));
        _stateMachine.AddTransition(barrierChargeState, idleState, new FuncPredicate(() => !_isSettingBarrier));
        _stateMachine.AddTransition(idleState, attackState, new FuncPredicate(() => _isAttacking));
        _stateMachine.AddTransition(locomotionState, attackState, new FuncPredicate(() => _isAttacking));
        _stateMachine.AddTransition(attackState, idleState, new FuncPredicate(() => !_isAttacking));



        _stateMachine.SetState(idleState);
    }

    private void OnEnable()
    {
        _inputHandler.OnPressAttack.AddListener(TryToAttack);
        _inputHandler.OnPressBarrier.AddListener(TryToSetBarrier);
    }

    private void OnDisable()
    {
        _inputHandler.OnPressAttack.RemoveAllListeners();
        _inputHandler.OnPressBarrier.RemoveAllListeners();
    }

    private void Update()
    {
        _stateMachine.Update();
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        float moveIntensity = Mathf.Abs(_joystickInput.x) < Mathf.Abs(_joystickInput.y) ? Mathf.Abs(_joystickInput.y) : Mathf.Abs(_joystickInput.x);
        _animator.SetFloat(_speed, moveIntensity);
    }

    private void TryToSetBarrier()
    {
        if (_player.CanSetBarrier)
            StartCoroutine(BarrierSetup());
    }

    private IEnumerator BarrierSetup()
    {
        _isSettingBarrier = true;

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("BarrierCharge"))
            yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;
            
        _isSettingBarrier = false;
    }

    private void TryToAttack()
    {
        if (_player.CanAttack)
        {
            switch (AttackType)
            {
                case AttackType.Explosion:
                    StartCoroutine(ExplosionAttackProcess());
                    break;
            }
        }
    }

    private void TryToUltimate()
    {
        if (_player.CanUltimate)
        {
            switch (AttackType)
            {
                case AttackType.Explosion:
                    StartCoroutine(ExplosionUltimateProcess());
                    break;
            }
        }
    }

    private IEnumerator ExplosionUltimateProcess()
    {
        _isUltimating = true;
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ExplosionUltimate"))
            yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;

        _isUltimating = false;
    }


    private IEnumerator ExplosionAttackProcess()
    {
        _isAttacking = true;
        _currEffect.GetComponent<ParticleSystem>().Play();
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ExplosionAttack"))
            yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;
        _isAttacking = false;
    }

    public void Move()
    {
        _moveDirection = new Vector3(_joystickInput.x, 0, _joystickInput.y);
        float rotationAngle = transform.eulerAngles.y;
        if (_joystickInput != Vector2.zero)
            rotationAngle = Mathf.Atan2(_joystickInput.x, _joystickInput.y) * Mathf.Rad2Deg;
        if (!DOTween.IsTweening(transform))
            transform.DORotate(new Vector3(0, rotationAngle, 0), 0.1f);
        _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }
}
