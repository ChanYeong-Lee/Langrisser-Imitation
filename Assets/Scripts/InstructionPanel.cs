using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InstructionPanel : MonoBehaviour
{
    public BasicInstruction basicInstruction;
    public ClassInstruction classInstruction;
    private GameObject currentInstruction;
    private void Awake()
    {
        currentInstruction = basicInstruction.gameObject;
    }

    public void SetPanel(InstructionType instructionType)
    {
        switch (instructionType)
        {
            case InstructionType.Basic:
                ChangePanel(basicInstruction.gameObject);
                break;
            case InstructionType.Class:
                ChangePanel(classInstruction.gameObject);
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
