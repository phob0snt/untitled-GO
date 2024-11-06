using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class MapManager : MonoBehaviour
{
    [Inject] private LocalObjectsSpawner _localSpawner;
    [SerializeField] private float _locationUpdateInterval;

    public double TotalDistance { get; private set; }
    public float CurrentWalkSpeed { get; private set; }

    private const float MIN_EVENT_SPAWN_DISTANCE = 45F;
    private const float MAX_EVENT_SPAWN_DISTANCE = 135F;
    private const int MAX_EVENTS_AMOUNT = 4;
    private float _distanceFromLastEvent;
    private float _distanceForNextEvent;

    private double _previousLatitude;
    private double _previousLongitude;
    private float _previousTime;
    

    private void OnEnable()
    {
        EventManager.AddListener<LocatorReadyEvent>((e) => StartCalculations());
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<LocatorReadyEvent>((e) => StartCalculations());
    }

    private void StartCalculations()
    {
        _previousLatitude = Input.location.lastData.latitude;
        _previousLongitude = Input.location.lastData.longitude;
        _previousTime = Time.time;

        StartCoroutine(CalculateWalkSpeed());
        StartCoroutine(LocalEventsSpawnProcess());
    }

    private IEnumerator LocalEventsSpawnProcess()
    {
        while (true)
        {
            if (_distanceFromLastEvent == 0)
                _distanceForNextEvent = UnityEngine.Random.Range(MIN_EVENT_SPAWN_DISTANCE, MAX_EVENT_SPAWN_DISTANCE);
            _distanceFromLastEvent += CurrentWalkSpeed;
            if (_distanceFromLastEvent >= _distanceForNextEvent)
                _localSpawner.Spawn(CreateRandomEvent());
            yield return new WaitForSeconds(1);
        }
    }

    private LocalMapObject CreateRandomEvent()
    {
        int type = UnityEngine.Random.Range(0, Enum.GetNames(typeof(MapObjectType)).Length);
        LocalMapObject obj;
        switch ((MapObjectType)type)
        {
            default:
            case MapObjectType.Battle:
                obj = ScriptableObject.CreateInstance<LocalBattle>();
                break;
        }
        obj.Initialize();
        obj.RandomConfiguration();
        return obj;
    }

    private IEnumerator CalculateWalkSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(_locationUpdateInterval);

            double currentLatitude = Input.location.lastData.latitude;
            double currentLongitude = Input.location.lastData.longitude;
            float currentTime = Time.time;

            double distance = GeoDistanceCalculator.CalculateDistance(_previousLatitude, _previousLongitude, currentLatitude, currentLongitude);
            TotalDistance += distance;

            float timeDelta = currentTime - _previousTime;
            float speed = (float)(distance / timeDelta);

            _previousLatitude = currentLatitude;
            _previousLongitude = currentLongitude;
            _previousTime = currentTime;

            CurrentWalkSpeed = speed;
            Debug.Log("Speed: " + speed + " m/s");
            Debug.Log("Total Distance: " + TotalDistance + " meters");
        }
    }
}

public class GeoDistanceCalculator
{
    private const double EarthRadius = 6371000;

    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double dLat = (lat2 - lat1) * Mathf.Deg2Rad;
        double dLon = (lon2 - lon1) * Mathf.Deg2Rad;

        double a = Mathf.Sin((float)dLat / 2) * Mathf.Sin((float)dLat / 2) +
                   Mathf.Cos(Mathf.Deg2Rad * (float)lat1) * Mathf.Cos(Mathf.Deg2Rad * (float)lat2) *
                   Mathf.Sin((float)dLon / 2) * Mathf.Sin((float)dLon / 2);
        double c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1 - a)));

        return EarthRadius * c;
    }
}
