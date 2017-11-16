using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



[System.Serializable]
public enum eAction
{
    Left, Right, Special, Jump
}
public class PointerListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerMovement action;
    public eAction Direction;
   
    bool isPressed = false;
    bool isInside = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        isInside = true;
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        action.StopRunning();
        isPressed = false;
        isInside = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInside = true;
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        action.StopRunning();
        isInside = false;
    }
 
    void Update()
    {
        if (!GameManager.Instance.isLevelComplete && !GameManager.Instance.isPaused){
            if (!isInside)
                return;
    
            switch(Direction)
            {
                case eAction.Left:
                    action.MoveLeft();
                    break;
                case eAction.Right:
                    action.MoveRight();
                    break;
                case eAction.Special:
                    if (isPressed){
                        action.Special();
                        isPressed = false;
                    }
                    break;
                case eAction.Jump:
                    if (isPressed){
                        action.Jump();
                        isPressed = false;
                    }
                    break;
            }
        }
    }
}