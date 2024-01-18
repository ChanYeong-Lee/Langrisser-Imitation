using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleScenePointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Camera.main.GetComponent<BattleSceneCameraMove>().canMove = false;
        BattleManager.Instance.canSelect = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Camera.main.GetComponent<BattleSceneCameraMove>().canMove = true;
        BattleManager.Instance.canSelect = true;
    }
}
