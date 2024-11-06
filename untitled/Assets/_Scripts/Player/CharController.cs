using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Character), typeof(CharacterController ))]
public class CharController : MonoBehaviour, IPlayerController
{
    [Inject] private readonly ViewManager _viewManager;
    [Inject] private readonly PlayerLocationHandler _locationHandler;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _smoothRate;
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _moveLerpDuration = 1.5f;
    [SerializeField] private Transform _camera;
    
    private float _currentVelocity = 0f;

    private PlayerStateMachine _stateMachine;
    private readonly int _speed = Animator.StringToHash("Speed");


    private void Awake()
    {
        _stateMachine = new PlayerStateMachine();
        var idleState = new IdleState(this, _animator);
        var locomotionState = new LocomotionState(this, _animator);

        _stateMachine.AddTransition(idleState, locomotionState, new FuncPredicate(() => _animator.GetFloat(_speed) > 0));
        _stateMachine.AddTransition(locomotionState, idleState, new FuncPredicate(() => _animator.GetFloat(_speed) == 0));
        _stateMachine.SetState(idleState);
    }

    private void OnEnable()
    {
        EventManager.AddListener<LocatorReadyEvent>((e) => StartCoroutine(PositionUpdateProcess()));
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<LocatorReadyEvent>((e) => StartCoroutine(PositionUpdateProcess()));
    }

    private void Update()
    {
        if (_viewManager.IsCurrentView<MapView>())
        {
            try
            {
                Touch touch = Input.GetTouch(0);
                _camera.transform.eulerAngles = new Vector3(15, _camera.transform.eulerAngles.y + touch.deltaPosition.x, 0);
            }
            catch { }
        }
        
        _stateMachine.Update();
    }

    public void Move()
    {
        _animator.SetFloat(_speed, _currentVelocity);
    }

    private IEnumerator PositionUpdateProcess()
    {
        Vector3 startPos = transform.position;
        float timeElapsed = 0f;
        transform.DORotateQuaternion(Quaternion.LookRotation(_locationHandler.GetPlayerPos() - startPos), 0.2f);
        while (timeElapsed / _moveLerpDuration < 1)
        {
            Vector3 tempPos = transform.position;
            transform.position = Vector3.Lerp(startPos, _locationHandler.GetPlayerPos(), timeElapsed / _moveLerpDuration);
            _currentVelocity = Vector3.Distance(tempPos, transform.position) * 20 > 1 ? 1 : Vector3.Distance(tempPos, transform.position) * 20;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _currentVelocity = 0f;
        Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        yield return new WaitForSeconds(5);
    }
}
