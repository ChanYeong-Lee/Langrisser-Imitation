using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMapPointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Camera.main.GetComponent<WorldMapController>().canMove = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Camera.main.GetComponent<WorldMapController>().canMove = true;
    }
}
