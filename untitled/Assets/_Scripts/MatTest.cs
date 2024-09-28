using UnityEngine;

public class MatTest : MonoBehaviour
{
    private Renderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        foreach (var material in _renderer.materials)
        {
            Debug.Log(material);
        }
    }
}
