using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralCard : MonoBehaviour
{
    public General general;
    public Image generalCardHead;
    public Image rairityCard;
    public Image occupation;
    [HideInInspector] public Toggle toggle;
    public void Init()
    {
        toggle = GetComponent<Toggle>();
    }
    public void SetCard(General general)
    {
        this.general = general;
        this.generalCardHead.sprite = ResourceManager.Instance.GetGeneralResource(general.GeneralType).generalCardHead;
        this.rairityCard.sprite = ResourceManager.Instance.GetRairityResource(general.RairityType).rairityCard;
        this.occupation.sprite = ResourceManager.Instance.GetClassResource(general.ClassType).classIcon;
    }
}
