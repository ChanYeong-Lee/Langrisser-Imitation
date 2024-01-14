using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionOption : MonoBehaviour
{
    [HideInInspector] public Toggle toggle;
    private RectTransform rectTransform;
    private Vector2 originPos;
    public InstructionType InstructionType;
    public void Init()
    {
        toggle = GetComponent<Toggle>();
        rectTransform = GetComponent<RectTransform>();
        originPos = new Vector2(0, rectTransform.anchoredPosition.y);
        toggle.onValueChanged.AddListener(DisableBackground);
        toggle.onValueChanged.AddListener(MovePosition);
    }

    public void DisableBackground(bool isOn)
    {
        toggle.targetGraphic.enabled = !isOn;
    }
    public void MovePosition(bool isOn)
    {
        rectTransform.anchoredPosition = isOn ? new Vector2(-15, originPos.y) : originPos;
    }

}
