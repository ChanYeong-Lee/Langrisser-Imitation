using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

public class GeneralInstruction : MonoBehaviour
{
    [Header("General Instruction")]
    [Header("Instruction")]
    public TextMeshProUGUI className;
    public Image generalOccupation;
    public TextMeshProUGUI generalLevel;
    public TextMeshProUGUI generalName;
    public Image generalHPBar;
    public TextMeshProUGUI generalHP;
    public TextMeshProUGUI generalRange;
    public TextMeshProUGUI generalMoveCost;

    [Header("Stats")]
    public TextMeshProUGUI generalLife;
    public TextMeshProUGUI generalAttack;
    public TextMeshProUGUI generalDefense;

    [Header("Soldier Instruction")]
    [Header("Instruction")]
    public Image soldierSprite;
    public Image soldierOccupation;
    public TextMeshProUGUI soldierName;
    public Image soldierHPBar;
    public TextMeshProUGUI soldierHP;

    [Header("Stats")]
    public TextMeshProUGUI soldierLife;
    public TextMeshProUGUI soldierAttack;
    public TextMeshProUGUI soldierDefense;
    
    public List<GameObject> gameObjects;

    public void SetInstruction(MovingObject movingObject)
    {
        General general = movingObject.general;
        Soldier soldier = movingObject.soldier;
        GeneralResource generalResource = ResourceManager.Instance.GetGeneralResource(general.GeneralType);
        ClassResource classResource = ResourceManager.Instance.GetClassResource(general.ClassType);
        SoldierResource soldierResource = ResourceManager.Instance.GetSoldierResource(soldier.soldierType);

        className.text = classResource.className;
        generalOccupation.sprite = classResource.classIcon;
        generalLevel.text = general.Level.ToString();
        generalName.text = generalResource.generalName;
        generalHPBar.fillAmount = general.CurrentHP / general.maxHP;
        generalHP.text = $"{general.CurrentHP}/{general.maxHP}";
        generalRange.text = general.range.ToString();
        generalMoveCost.text = general.moveCost.ToString();

        generalLife.text = general.maxHP.ToString();
        generalAttack.text = general.damage.ToString();
        generalDefense.text = general.defense.ToString();

        soldierSprite.sprite = soldierResource.soldierSD;
        soldierOccupation.sprite = ResourceManager.Instance.GetClassResource(soldier.ClassType).classIcon;
        soldierName.text = soldierResource.soldierName;
        soldierHPBar.fillAmount = soldier.CurrentHP / soldier.maxHP;
        soldierHP.text = $"{soldier.CurrentHP}/{soldier.maxHP}";

        soldierLife.text = soldier.maxHP.ToString();
        soldierAttack.text = soldier.damage.ToString();
        soldierDefense.text = soldier.defense.ToString();
    }

    public void SetParent(RectTransform parent)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<RectTransform>().SetParent(parent);
        }
    }
}
