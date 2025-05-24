using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IClickableObject : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public UnityEvent OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Click");
        OnClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

   
}
