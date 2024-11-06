using UnityEngine;

public class LocalObjectsSpawner : MonoBehaviour
{
    public void Spawn(LocalMapObject obj)
    {
        Vector3 pos = CalculateRandomPos();
        Instantiate(obj, pos, Quaternion.identity).Initialize();
    }

    private Vector3 CalculateRandomPos()
    {
        Vector3 playerPos = transform.position;
        return new Vector3(Random.Range(playerPos.x - 10f, playerPos.x + 10f),
                           playerPos.y,
                           Random.Range(playerPos.z - 10f, playerPos.z + 10f));
    }
}
