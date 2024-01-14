using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectElement : MonoBehaviour/*, IDragHandler, IBeginDragHandler, IEndDragHandler*/
{
    public General general;
    public Image characterIcon;
    public Image classIcon;
    public TextMeshProUGUI levelText;

    public Vector2 prevPos;
    public RectTransform rectTransform;
    bool isDrag;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();  
    }

    public void SetGeneral(General general)
    {
        this.general = general;
        characterIcon.sprite = ResourceManager.Instance.GetGeneralResource(general.GeneralType).GetGeneralIcon(general.RairityType);
        classIcon.sprite = ResourceManager.Instance.GetClassResource(general.ClassType).classIcon;
        levelText.text = general.Level.ToString();
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    prevPos = rectTransform.anchoredPosition;
    //    if (eventData.delta.y > 0) isDrag = true;
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    print(eventData.delta);
    //    if (isDrag)
    //    {
    //        rectTransform.anchoredPosition += eventData.delta;
    //    }
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    //    if (hit.collider != null && hit.collider.TryGetComponent<AllyObject>(out AllyObject ally))
    //    {
    //        if (ally.general == null)
    //        {
    //            ally.SetGeneral(general);
    //            Destroy(gameObject);
    //        }
    //    }
    //    rectTransform.anchoredPosition = prevPos;
    //    isDrag = false;
    //}

}
