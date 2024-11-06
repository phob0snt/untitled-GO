using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class View : MonoBehaviour
{
    [SerializeField] protected List<UIElement> _UIElements;
    public virtual void Initialize()
    {
        foreach (var element in _UIElements)
            element.Initialize();
    }
    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide() => gameObject.SetActive(false);
    [Inject] protected ViewManager _viewManager;
}
