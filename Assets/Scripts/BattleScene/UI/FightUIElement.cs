using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUIElement : MonoBehaviour
{
    private MovingObject movingObject; 
    private FightType fightType;

    [Header("Field")]
    public Image generalSD;
    public List<Image> soldiersSD;

    [Header("Instruction")]
    public Image generalSpine;
    public Image generalHPBar;
    public Image soldierHPBar;
    public TextMeshProUGUI generalHPText;
    public TextMeshProUGUI soldierHPText;

    public void SetElement(MovingObject movingObject, FightType fightType)
    {
        this.movingObject = movingObject;
        this.fightType = fightType;

        General general = movingObject.general;
        Soldier soldier = movingObject.soldier; 
        GeneralResource generalResource = ResourceManager.Instance.GetGeneralResource(general.GeneralType);
        SoldierResource soldierResource = ResourceManager.Instance.GetSoldierResource(soldier.soldierType);

        #region SetField
        generalSD.sprite = generalResource.generalSD;
        if (soldier.Alive)
        {
            foreach (Image image in soldiersSD)
            {
                image.sprite = soldierResource.soldierSD;
                image.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Image image in soldiersSD)
            {
                image.gameObject.SetActive(false);
            }
        }
        #endregion

        #region SetInstruction
        generalSpine.sprite = generalResource.generalSD;
        float generalHPAmount = general.CurrentHP / general.maxHP;
        float soldierHPAmount = soldier.CurrentHP / soldier.maxHP;
        generalHPBar.fillAmount = generalHPAmount;
        soldierHPBar.fillAmount = soldierHPAmount;
        generalHPText.text = general.CurrentHP.ToString();
        soldierHPText.text = soldier.CurrentHP.ToString();
        #endregion
    }

    public void UpdateElement(float damage)
    {
        StartCoroutine(FightCoroutine(damage));
    }

    IEnumerator FightCoroutine(float damage)
    {

        float currentGeneralHP = movingObject.general.CurrentHP;
        float maxGeneralHP = movingObject.general.maxHP;
        float currentSoldierHP = movingObject.soldier.CurrentHP;
        float maxSoldierHP = movingObject.soldier.maxHP;
        float currentTime = 0;
        while (true)
        {
            float currentDamage = damage * Time.deltaTime / 2;
            currentTime += Time.deltaTime;
            if (currentTime >= 2f) break;
            if (currentGeneralHP <= 0) break; ;
            if (currentSoldierHP - currentDamage > 0)
            {
                currentSoldierHP -= currentDamage;
            }
            else
            {
                float remainDamage = currentDamage - currentSoldierHP;
                currentSoldierHP = 0;
                currentGeneralHP -= remainDamage;
            }
            generalHPBar.fillAmount = currentGeneralHP / maxGeneralHP;
            soldierHPBar.fillAmount = currentSoldierHP / maxSoldierHP;
            generalHPText.text = $"{(int)currentGeneralHP}";
            soldierHPText.text = $"{(int)currentSoldierHP}";
            
            yield return null;
        }
        print(currentTime);
        yield return new WaitForSeconds(1f);
        if(fightType == FightType.Attacker) movingObject.state = MovingObject.State.Wait;
    }
}
