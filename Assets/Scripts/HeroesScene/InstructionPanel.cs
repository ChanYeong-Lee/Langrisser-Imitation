using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InstructionPanel : MonoBehaviour
{
    public BasicInstruction basicInstruction;
    public SoldierInstruction soldierInstruction;
    private GameObject currentInstruction;
    public void Init()
    {
        basicInstruction.gameObject.SetActive(false);
        soldierInstruction.gameObject.SetActive(false);
        basicInstruction.Init();
        soldierInstruction.Init();
    }

    public void SetPanel(InstructionType instructionType)
    {
        switch (instructionType)
        {
            case InstructionType.Basic:
                ChangePanel(basicInstruction.gameObject);
                break;
            case InstructionType.Class:
                ChangePanel(soldierInstruction.gameObject);
                break;
        }
    }

    private void ChangePanel(GameObject panel)
    {
        if (currentInstruction != null)
        {
            currentInstruction.SetActive(false);
        }
        currentInstruction = panel.gameObject;
        panel.gameObject.SetActive(true);
    }
}
