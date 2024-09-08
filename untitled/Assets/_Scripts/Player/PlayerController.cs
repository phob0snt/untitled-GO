using UnityEngine;
using DG.Tweening;
using Zenject;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [Inject] private readonly FightManager _fightManager;
    [Inject] private readonly InputHandler _inputHandler;

    [SerializeField] private Animator _animator;
    [SerializeField] private float _smoothRate;
    [SerializeField] private float _moveSpeed = 6f;

    private Vector2 _joystickInput => _inputHandler.GetJoystickInput();
    private Vector3 _moveDirection;

    private bool _isSettingBarrier;

    private CharacterController _controller;
    private Player _player;
    private PlayerStateMachine _stateMachine;

    private readonly int _speed = Animator.StringToHash("Speed");
    private readonly int _barrierCharge = Animator.StringToHash("BarrierCharge");

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _player = GetComponent<Player>();

        _stateMachine = new PlayerStateMachine();
        var locomotionState = new LocomotionState(this, _animator);
        var barrierChargeState = new BarrierChargeState(this, _animator);


        _stateMachine.AddTransition(locomotionState, barrierChargeState, new FuncPredicate(() => _isSettingBarrier));
        _stateMachine.AddTransition(barrierChargeState, locomotionState, new FuncPredicate(() => !_isSettingBarrier));

        _stateMachine.SetState(locomotionState);
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
    }

    private void AnimateMovement()
    {
        float moveIntensity = Mathf.Abs(_joystickInput.x) < Mathf.Abs(_joystickInput.y) ? Mathf.Abs(_joystickInput.y) : Mathf.Abs(_joystickInput.x);
        _animator.SetFloat(_speed, moveIntensity);
    }

    private void TryToSetBarrier()
    {
        if (_player.CanSetBarrier)
        {
            StartCoroutine(BarrierSetup());
        }
    }

    private IEnumerator BarrierSetup()
    {
        _isSettingBarrier = true;

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("BarrierCharge"))
            yield return null;
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            //if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 0.7f)
            //    _player.SetBarrier();
            yield return null;
        }
            
        _isSettingBarrier = false;
    }


    private void TryToAttack()
    {

    }

    public void Move()
    {
        _moveDirection = new Vector3(_joystickInput.x, 0, _joystickInput.y);
        float rotationAngle = Mathf.Atan2(_joystickInput.x, _joystickInput.y) * Mathf.Rad2Deg;
        if (!DOTween.IsTweening(transform))
            transform.DORotate(new Vector3(0, rotationAngle, 0), 0.1f);
        //transform.eulerAngles = new Vector3(0, rotationAngle, 0);
        AnimateMovement();
        _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }
}
