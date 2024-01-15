using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicInstruction : MonoBehaviour
{
    public Image occupationFlag;
    
    [Header("ExpBar")]
    public Image expBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    
    [Header("General Instruction")]
    public Image generalSprite;
    public TextMeshProUGUI lifeStat;
    public TextMeshProUGUI attackStat;
    public TextMeshProUGUI defenseStat;

    [Header("Class Instruction")]
    public TextMeshProUGUI className;
    public TextMeshProUGUI classDescription;
    public TextMeshProUGUI classRange;
    public TextMeshProUGUI classMoveCost;

    public void Init()
    {
        HeroManager.Instance.onChangeGeneral.AddListener(UpdateInstruction);
    }

    private void OnEnable()
    {
        UpdateInstruction();
    }

    public void UpdateInstruction()
    {
        General general = HeroManager.Instance.currentGeneral;
        GeneralResource generalResource = ResourceManager.Instance.GetGeneralResource(general.GeneralType);
        ClassResource classResource = ResourceManager.Instance.GetClassResource(general.ClassType);
        
        occupationFlag.sprite = classResource.classFlag;
        
        expBar.fillAmount = general.CurrentExp / general.MaxExp;
        levelText.text = $"{general.Level:D2}";
        expText.text = $"{general.CurrentExp}/{general.MaxExp}";

        generalSprite.sprite = generalResource.generalSD;
        lifeStat.text = $"{general.maxHP}";
        attackStat.text = $"{general.damage}";
        defenseStat.text = $"{general.defense}";

        className.text = classResource.className;
        classDescription.text = classResource.classDescription;
        classRange.text = $"{general.range}";
        classMoveCost.text = $"{general.moveCost}";

    }
}
