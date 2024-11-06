using System.Collections;
using UnityEngine;

public class PlayerLocationHandler : MonoBehaviour
{
    private const float MIN_LAT = 55.585469f;
    private const float MAX_LAT = 55.908336f;
    private const float MIN_LON = 37.381405f;
    private const float MAX_LON = 37.892572f;

    private readonly Vector2 MinMapCords = new(-3273.2f, -3794.4f);
    private readonly Vector2 MaxMapCords = new(3149f, 3401f);

    private float _latScale;
    private float _lonScale;

    private void Awake()
    {
        _latScale = (MaxMapCords.y - MinMapCords.y) / (MAX_LAT - MIN_LAT);
        _lonScale = (MaxMapCords.x - MinMapCords.x) / (MAX_LON - MIN_LON);
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
            EventManager.Broadcast(Events.LocatorReadyEvent);
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
    }

    public Vector3 GetPlayerPos()
    {
#if UNITY_EDITOR
        float userZ = MinMapCords.y + (MAX_LAT - 55.74334f) * _latScale;
        float userX = MinMapCords.x + (MAX_LON - 37.872f) * _lonScale;
#else
        float userZ = MinMapCords.y + (MAX_LAT - Input.location.lastData.latitude) * _latScale;
        float userX = MinMapCords.x + (MAX_LON - Input.location.lastData.longitude) * _lonScale;
#endif
        return new Vector3(userX, 30, userZ);
    }
}
