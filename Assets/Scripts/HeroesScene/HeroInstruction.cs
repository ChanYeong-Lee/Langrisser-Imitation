using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInstruction : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image rairity;
    private void Start()
    {
        HeroManager.Instance.onChangeGeneral.AddListener(UpdateInstruction);
    }

    public void UpdateInstruction()
    {
        nameText.text = ResourceManager.Instance.GetGeneralResource(HeroManager.Instance.currentGeneral.GeneralType).generalName;
        rairity.sprite = ResourceManager.Instance.GetRairityResource(HeroManager.Instance.currentGeneral.RairityType).rairityIcon;
    }
}