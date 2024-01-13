using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<General> generalPrefabs;
    public List<Soldier> soldierPrefabs;
  
    public General GetGeneral(GeneralType generalType)
    {
        General general = generalPrefabs.Find((a) => a.GeneralType == generalType);
        return general;
    }

    public Soldier GetSoldier(SoldierType soldierType)
    {
        Soldier soldier = soldierPrefabs.Find((a) => a.soldierType == soldierType);
        return soldier;
    }
}
