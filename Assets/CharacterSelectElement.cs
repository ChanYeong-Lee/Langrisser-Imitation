using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectElement : MonoBehaviour
{
    public General general;
    public Image characterIcon;
    public Image classIcon;
    public TextMeshProUGUI levelText;
    DragEventHandler dragEventHandler;
    private void Awake()
    {
        dragEventHandler = GetComponent<DragEventHandler>();
        if (dragEventHandler == null)
        {
            dragEventHandler = gameObject.AddComponent<DragEventHandler>();
        }
        dragEventHandler.fixedPos = true;
    }
    public void SetGeneral(General general)
    {
        this.general = general;
        characterIcon.sprite = DataManager.Instance.GetGeneralIcon(general);
        classIcon.sprite = DataManager.Instance.GetClassIcon(general);
        levelText.text = general.Level.ToString();
        dragEventHandler.OnEndDragEvent.AddListener(OnEndDrag);
    }

    private void OnEndDrag()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.TryGetComponent<AllyObject>(out AllyObject ally))
        {
            ally.SetGeneral(general);
            Destroy(gameObject);
        }
    }
}
