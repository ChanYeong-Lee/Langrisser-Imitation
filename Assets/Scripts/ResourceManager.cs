using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class GeneralIcon
{
    public RairityType rairityType;
    public Sprite generalIcon;
}

[Serializable] public class ClassResource
{
    public string className;
    [TextArea(0, 3)] public string classDescription;
    public ClassType classType;
    public Sprite classIcon;
    public Sprite classFlag;
    public ClassType advantage;
    public ClassType weakness;
}

[Serializable]
public class RairityResource
{
    public RairityType rairityType;
    public Sprite rairityIcon;
    public Sprite rairityCard;
}

[Serializable]
public class SoldierResource
{
    public SoldierType soldierType;
    public string soldierName;
    public string soldierDescription;
    public Sprite soldierSD;
}

[Serializable]
public class GeneralResource
{
    public string generalName;
    public GeneralType generalType;
    public List<GeneralIcon> generalIcons;
    public Sprite generalPaint;
    public Sprite generalSD;
    public Sprite generalCardHead;
    
    public Sprite GetGeneralIcon(RairityType rairtyType)
    {
        return generalIcons.Find((a) => a.rairityType == rairtyType).generalIcon;
    }
}
public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;
    public static ResourceManager Instance { get { return instance; } }

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

    public List<GeneralResource> generalResources;
    public List<SoldierResource> soldierResources;
    public List<ClassResource> classResources;
    public List<RairityResource> rairityResources;
    public GeneralResource GetGeneralResource(GeneralType generalType)
    {
        GeneralResource generalResource = generalResources.Find((a) => a.generalType == generalType);
        return generalResource;
    }
    public SoldierResource GetSoldierResource(SoldierType soldierType)
    {
        SoldierResource soldierResource = soldierResources.Find((a) => a.soldierType == soldierType);
        return soldierResource;
    }
    public ClassResource GetClassResource(ClassType classType)
    {
        return classResources.Find((a) => a.classType == classType);
    }
    public RairityResource GetRairityResource(RairityType rairityType)
    {
        return rairityResources.Find((a) => a.rairityType == rairityType);
    }
}
