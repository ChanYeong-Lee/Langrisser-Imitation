using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DragEventHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public UnityEvent OnEndDragEvent;
    public RectTransform rectTransform;
    public bool fixedPos;
    private Vector2 prevPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevPos = rectTransform.anchoredPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke();
        if (fixedPos)
        {
            rectTransform.anchoredPosition = GetComponentInParent<RectTransform>().anchoredPosition;
        }
        print("NotGOod");
    }
}
