using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Character), typeof(CharacterController ))]
public class CharController : MonoBehaviour, IPlayerController
{
    [Inject] private readonly ViewManager _viewManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _smoothRate;
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _moveLerpDuration = 1.5f;
    [SerializeField] private Transform _camera;

    private const float MIN_LAT = 55.585469f;
    private const float MAX_LAT = 55.908336f;
    private const float MIN_LON = 37.381405f;
    private const float MAX_LON = 37.892572f;
    private float _currentVelocity = 0f;

    private readonly Vector2 MinMapCords = new(-3273.2f, -3794.4f);
    private readonly Vector2 MaxMapCords = new(3149f, 3401f);
    private float _latScale;
    private float _lonScale;

    private CharacterController _controller;
    private Character _char;
    private PlayerStateMachine _stateMachine;
    private readonly int _speed = Animator.StringToHash("Speed");


    private void Awake()
    {
        _latScale = (MaxMapCords.y - MinMapCords.y) / (MAX_LAT - MIN_LAT);
        _lonScale = (MaxMapCords.x - MinMapCords.x) / (MAX_LON - MIN_LON);

        _stateMachine = new PlayerStateMachine();
        var locomotionState = new LocomotionState(this, _animator);

        _stateMachine.AddTransition(locomotionState, locomotionState, new FuncPredicate(() => false));
        _stateMachine.SetState(locomotionState);
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
        Debug.Log(_currentVelocity);
    }

    private IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
            Debug.Log("GEO DISABLED");

        Input.location.Start(5, 5);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Debug.Log("Trying to get location");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
        StartCoroutine(GetLocation());
    }

    private Material mat;
    private IEnumerator GetLocation()
    {
        //55.743340, 37.872020  55.744895, 37.864115
        float lat = 55.74334f;
        float lon = 37.872f;
        while (true)
        {
            //float userZ = MinMapCords.x + (55.7433558f - MIN_LAT) * _latScale;
            //float userX = MinMapCords.y + (MAX_LON - 37.8718856f) * _lonScale;
            //float userZ = MinMapCords.x + (55.744457f - MIN_LAT) * _latScale;
            //float userX = MinMapCords.y + (MAX_LON - 37.865963f) * _lonScale;
#if UNITY_EDITOR
            float userZ = MinMapCords.y + (MAX_LAT - 55.74334f) * _latScale;
            float userX = MinMapCords.x + (MAX_LON - 37.872f) * _lonScale;
#else
            float userZ = MinMapCords.y + (MAX_LAT - Input.location.lastData.latitude) * _latScale;
            float userX = MinMapCords.x + (MAX_LON - Input.location.lastData.longitude) * _lonScale;
#endif
            Vector3 startPos = transform.position;
            float timeElapsed = 0f;
            transform.DORotateQuaternion(Quaternion.LookRotation(new Vector3(userX, 30, userZ) - startPos), 0.2f);
            while (timeElapsed / _moveLerpDuration < 1)
            {
                Vector3 tempPos = transform.position;
                transform.position = Vector3.Lerp(startPos, new Vector3(userX, 30, userZ), timeElapsed / _moveLerpDuration);
                _currentVelocity = Vector3.Distance(tempPos, transform.position) * 20 > 1 ? 1 : Vector3.Distance(tempPos, transform.position) * 20;
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            _currentVelocity = 0f;
            //transform.position = new Vector3(userX, 30, userZ);
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            yield return new WaitForSeconds(5);
            //userZ = MinMapCords.y + (MAX_LAT - 55.74354f) * _latScale;
            //userX = MinMapCords.x + (MAX_LON - 37.8725f) * _lonScale;
            //startPos = transform.position;
            //timeElapsed = 0f;
            //transform.DORotateQuaternion(Quaternion.LookRotation(new Vector3(userX, 30, userZ) - startPos), 0.2f);
            //while (timeElapsed / _moveLerpDuration < 1)
            //{
            //    Vector3 tempPos = transform.position;
            //    transform.position = Vector3.Lerp(startPos, new Vector3(userX, 30, userZ), timeElapsed / _moveLerpDuration);
            //    _currentVelocity = Vector3.Distance(tempPos, transform.position) * 20 > 1 ? 1 : Vector3.Distance(tempPos, transform.position) * 20;
            //    timeElapsed += Time.deltaTime;
            //    yield return null;
            //}
            //_currentVelocity = 0f;
            //yield return new WaitForSeconds(5);
        }
    }

    //private IEnumerator Move(Vector3 targetPos)
    //{
        
    //}
}
