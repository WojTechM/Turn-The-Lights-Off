using UnityEngine;
using UnityEngine.EventSystems;

public class JoyButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool IsPressed;

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
    }
}