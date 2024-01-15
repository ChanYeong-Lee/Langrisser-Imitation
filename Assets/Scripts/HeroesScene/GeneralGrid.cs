using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralGrid : MonoBehaviour
{
    public List<GeneralCard> generalCards;
    public GeneralCard prefab;
    private ToggleGroup toggleGroup;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        foreach (General general in PlayerData.Instance.generals)
        {
            GeneralCard newCard = Instantiate(prefab, transform);
            newCard.Init();
            newCard.toggle.group = toggleGroup;
            newCard.SetCard(general);
            newCard.toggle.onValueChanged.AddListener(ChangeCard);
        }
    }

    public void ChangeCard(bool isOn)
    {
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            if (toggle.isOn)
            {
                HeroManager.Instance.ChangeGeneral(toggle.GetComponent<GeneralCard>().general);
            }
        }
    }
}