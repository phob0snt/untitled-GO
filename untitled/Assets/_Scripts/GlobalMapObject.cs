using UnityEngine;

public abstract class GlobalMapObject : MonoBehaviour
{
    /// <summary>
    /// Object placement on map (latitude, longitude)
    /// </summary>
    public Vector2 Coords { get; private set; }
}
