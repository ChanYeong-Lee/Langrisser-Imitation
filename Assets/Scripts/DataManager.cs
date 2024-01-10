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
    public List<GeneralIcon> generalIconSprites;
    public List<ClassIcon> classIconSprites;
  
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

    public Sprite GetGeneralIcon(General general)
    {
        return generalIconSprites.Find((a) => (a.generalType == general.GeneralType && a.rairityType == general.RairtyType)).generalIcon;
    }
    public Sprite GetClassIcon(General general)
    {
        return classIconSprites.Find((a) => (a.classType == general.classType)).classIcon;
    }
}

[Serializable]
public class GeneralIcon
{
    public GeneralType generalType;
    public RairityType rairityType;
    public Sprite generalIcon;
}

[Serializable]
public class ClassIcon
{
    public ClassType classType;
    public Sprite classIcon;
}
