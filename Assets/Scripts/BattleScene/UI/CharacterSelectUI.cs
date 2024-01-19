using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public void Init()
    {
        foreach (General general in PlayerData.Instance.generals)
        {
            GenerateElement(general, (Vector2)transform.position);
        }
    }

    public CharacterSelectElement GenerateElement(General general, Vector2 pos)
    {
        CharacterSelectElement element = Instantiate(prefab, pos, Quaternion.identity);
        element.transform.localScale = Vector3.one;
        element.transform.SetParent(parent, false);
        element.SetGeneral(general);
        return element;
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
        float different = (float)1600 / Screen.currentResolution.width;
        Debug.Log(different);
        if (onMoveElement && currentElement != null)
        {
            currentElement.rectTransform.anchoredPosition += eventData.delta * different;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onMoveElement)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.TryGetComponent(out BattleMapCell cell))
            {
                if (cell.movingObject != null && cell.movingObject is AllyObject)
                {
                    if (cell.movingObject.general != null)
                    {
                        GenerateElement(cell.movingObject.general, transform.position);
                    }

                    cell.movingObject.GetComponent<AllyObject>().SetGeneral(currentElement.general);
                    StartCoroutine(generateDelayCoroutine());
                    Destroy(currentElement.gameObject);
                }
            }
            currentElement.rectTransform.anchoredPosition = currentElement.prevPos;
        }
        scrollRect.enabled = true;
        currentElement = null;
        onMoveElement = false;
    }

    IEnumerator generateDelayCoroutine()
    {
        BattleManager.Instance.canSelect = false;
        yield return null;
        BattleManager.Instance.canSelect = true;
    }
}
