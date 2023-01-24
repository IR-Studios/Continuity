using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Clickable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;
    public UnityEvent onRightShift;
    public UnityEvent onMouseHover;
    public UnityEvent onMouseHoverExit;
 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeft.Invoke();
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && eventData.button == PointerEventData.InputButton.Right)
        {
            onRight.Invoke();
            Debug.Log("Right Click");
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        } else if (Input.GetKey(KeyCode.LeftShift) && eventData.button == PointerEventData.InputButton.Right) 
        {
            onRightShift.Invoke();
            Debug.Log("Right Click Shift");
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        onMouseHover.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        onMouseHoverExit.Invoke();
    }
} 
