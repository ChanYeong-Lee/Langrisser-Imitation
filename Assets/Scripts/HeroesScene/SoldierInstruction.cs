using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInstruction : MonoBehaviour
{
    [Header("Soldier Instruction")]
    public Image soldierOccupation;
    public TextMeshProUGUI soldierNameText;
    public Image soldierSprite;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI moveCostText;

    [Header("Soldier Stats")]
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;

    [Header("Description")]
    public TextMeshProUGUI advantageText;
    public TextMeshProUGUI weaknessText;
    public TextMeshProUGUI descriptionText;

    public SoldierGrid soldierGrid;

    public void Init()
    {
        HeroManager.Instance.onChangeGeneral.AddListener(UpdateInstruction);
        HeroManager.Instance.onChangeSoldier.AddListener(UpdateInstruction);
        soldierGrid.Init();
    }

    private void UpdateInstruction()
    {
        General general = HeroManager.Instance.currentGeneral;
        Soldier soldier = general.Soldier;
        SoldierResource soldierResource = ResourceManager.Instance.GetSoldierResource(soldier.soldierType);
        ClassResource classResource = ResourceManager.Instance.GetClassResource(soldier.ClassType);

        soldierOccupation.sprite = classResource.classIcon;
        soldierNameText.text = soldierResource.soldierName;
        soldierSprite.sprite = soldierResource.soldierSD;
        rangeText.text = soldier.range.ToString();
        moveCostText.text = soldier.moveCost.ToString();

        lifeText.text = soldier.maxHP.ToString();
        attackText.text = soldier.damage.ToString();
        defenseText.text = soldier.defense.ToString();

        advantageText.text = ResourceManager.Instance.GetClassResource(classResource.advantage).className;
        weaknessText.text = ResourceManager.Instance.GetClassResource(classResource.weakness).className;
        descriptionText.text = soldierResource.soldierDescription;

    }
}