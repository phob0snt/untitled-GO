using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ContentRectAutoExpand : MonoBehaviour
{
    private RectTransform _rect => GetComponent<RectTransform>();
    private const int ROW_HEIGHT = 357;
    public void UpdateRectSize()
    {
        int yCapability = (int)_rect.sizeDelta.y / ROW_HEIGHT;
        _rect.offsetMin = new Vector2(0, -(transform.childCount / 3 - yCapability + 1) * ROW_HEIGHT);
        Debug.Log(_rect.offsetMin);
    }
}
