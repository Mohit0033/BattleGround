using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float h = 0;
    public float v = 0;

    private UserControl control;

    private void Start()
    {
        control = FindObjectOfType<UserControl>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        control.TouchInput(h, v);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        control.TouchInput(0, 0);
    }
}
