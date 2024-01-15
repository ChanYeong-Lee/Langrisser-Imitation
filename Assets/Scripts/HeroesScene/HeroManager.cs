using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroManager : MonoBehaviour
{
    public static HeroManager Instance { get; private set; }
    public General currentGeneral;
    public UnityEvent onChangeGeneral;
    public UnityEvent onChangeSoldier;
    private void Awake()
    {
        Instance = this;
        currentGeneral = PlayerData.Instance.generals[0];
        HeroesUIManager.Instance.Init();
    }

    public void ChangeGeneral(General general)
    {
        currentGeneral = general;
        onChangeGeneral?.Invoke();
    }

    public void ChangeSoldier(Soldier soldier)
    {
        currentGeneral.Soldier = soldier;
        onChangeSoldier?.Invoke();  
    }
}