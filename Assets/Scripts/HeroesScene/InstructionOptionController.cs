using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InstructionType
{
    Basic,
    Class
}
public class InstructionOptionController : MonoBehaviour
{
    public InstructionPanel panel;
    private InstructionOption[] instructionOptions;
    private ToggleGroup toggleGroup;
    bool prepared;

    public void Init()
    {
        instructionOptions = GetComponentsInChildren<InstructionOption>();
        toggleGroup = GetComponent<ToggleGroup>();
        foreach (InstructionOption option in instructionOptions)
        {
            option.Init();
            option.toggle.group = toggleGroup;
            option.toggle.onValueChanged.AddListener(ChangeOption);
        }
        prepared = true;
    }
    
    private void OnEnable()
    {
        if(prepared)
        ChangeOption(true);
    }

    public void ChangeOption(bool isOn)
    {
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            if (toggle.isOn)
            {
                panel.SetPanel(toggle.GetComponent<InstructionOption>().InstructionType);
            }
        }
    }
}
