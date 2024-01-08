using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(name + "GameObject Click in Progress");
        Camera.main.GetComponent<WorldMapController>().canMove = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(name + "No longer being clicked");
        Camera.main.GetComponent<WorldMapController>().canMove = true;
    }
}
