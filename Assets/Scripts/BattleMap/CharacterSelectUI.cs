using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public List<CharacterSelectElement> elements;
    public CharacterSelectElement prefab;
    public CharacterSelectElement currentElement;
    public Transform parent;
    bool onMoveElement;
    ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        foreach (General general in PlayerData.Instance.generals)
        {
            GenerateElement(general, (Vector2)transform.position);
        }
    }

    private CharacterSelectElement GenerateElement(General general, Vector2 pos)
    {
        CharacterSelectElement element = Instantiate(prefab, pos, Quaternion.identity);
        element.transform.localScale = Vector3.one;
        element.transform.SetParent(parent, false);
        element.SetGeneral(general);
        return element;
    }

    public void TestSelection()
    {
        elements[0].SetGeneral(PlayerData.Instance.generals[0]);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
   
        // UI 요소에 레이캐스트 수행
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        foreach (RaycastResult raycastResult in result)
        {

            // result 변수를 통해 UI 요소에 대한 정보에 접근 가능
            if (raycastResult.gameObject != null && raycastResult.gameObject.TryGetComponent(out currentElement) && eventData.delta.y > 0)
            {
                currentElement.prevPos = currentElement.rectTransform.anchoredPosition;
                onMoveElement = true;
                scrollRect.enabled = false;
                break;
                // 여기서 result 변수를 사용하여 필요한 작업 수행
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onMoveElement && currentElement != null)
        {
            currentElement.rectTransform.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onMoveElement)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.TryGetComponent(out AllyObject ally))
            {
                if (ally.general != null)
                {
                    GenerateElement(ally.general, transform.position);
                }
                ally.SetGeneral(currentElement.general);
                Destroy(currentElement.gameObject);
            }
            currentElement.rectTransform.anchoredPosition = currentElement.prevPos;
        }
        scrollRect.enabled = true;
        currentElement = null;
        onMoveElement = false;
    }
}
