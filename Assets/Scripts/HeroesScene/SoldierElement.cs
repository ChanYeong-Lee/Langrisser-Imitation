using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierElement : MonoBehaviour
{
    [HideInInspector] public Soldier soldier;
    [HideInInspector] public Toggle toggle;
    public TextMeshProUGUI soldierName;
    public Image soldierSprite;

    public void Init()
    {
        toggle = GetComponent<Toggle>();
    }

    public void SetSoldier(Soldier soldier)
    {
        this.soldier = soldier;
        SoldierResource soldierResource = ResourceManager.Instance.GetSoldierResource(soldier.soldierType);
        soldierName.text = soldierResource.soldierName;
        soldierSprite.sprite = soldierResource.soldierSD;
    }
}
