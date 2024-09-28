using UnityEngine;

public abstract class LocalMapObject : MonoBehaviour
{
    public virtual void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
