using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoldierGrid : MonoBehaviour
{
    public SoldierElement prefab;
    public ToggleGroup toggleGroup;
    public List<SoldierElement> elements;
    Soldier selectedSoldier;
    public void Init()
    {
        selectedSoldier = HeroManager.Instance.currentGeneral.Soldier;
        toggleGroup = GetComponent<ToggleGroup>();

        foreach (Soldier soldier in PlayerData.Instance.soldiers)
        {
            SoldierElement soldierElement = Instantiate(prefab, transform);
            soldierElement.Init();
            soldierElement.SetSoldier(soldier);
            soldierElement.toggle.group = toggleGroup;
            elements.Add(soldierElement);
            soldierElement.toggle.onValueChanged.AddListener(ChangeSoldier);
            //if (soldier == selectedSoldier) soldierElement.toggle.isOn = true;
        }
        HeroManager.Instance.onChangeGeneral.AddListener(UpdateGrid);
    }

    public void UpdateGrid()
    {
        Debug.Log("Update Grid");
        Debug.Log($"현재 병사는 {selectedSoldier.soldierType}");

        selectedSoldier = HeroManager.Instance.currentGeneral.Soldier;

        foreach (SoldierElement element in elements)
        {
            Debug.Log(element.soldierName.text);
            if (element.soldier == selectedSoldier)
            {
                Debug.Log($"{element.soldierName.text} is now.");
                element.toggle.isOn = true;
                break;
            }
        }
    }

    public void ChangeSoldier(bool isOn)
    {
            foreach (Toggle toggle in toggleGroup.ActiveToggles())
            {
                if (toggle.isOn)
                {
                    HeroManager.Instance.ChangeSoldier(toggle.GetComponent<SoldierElement>().soldier);
                }
            }
    }
}
