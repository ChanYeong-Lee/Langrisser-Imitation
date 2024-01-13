using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class GeneralIcon
{
    public RairityType rairityType;
    public Sprite generalIcon;
}

[Serializable] public class ClassIcon
{
    public ClassType classType;
    public Sprite classIcon;
}

[Serializable]
public class RairityIcon
{
    public RairityType rairityType;
    public Sprite rairityIcon;
}

[Serializable] public class GeneralResource
{
    public GeneralType generalType;
    public List<GeneralIcon> generalIcons;
    public Sprite generalPaint;
    public Sprite generalCardHead;
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
    public List<ClassIcon> classIconSprites;
    public List<RairityIcon> rairityIconSprites;
    public Sprite GetGeneralIcon(General general)
    {
        GeneralResource generalResource = generalResources.Find((a) => (a.generalType == general.GeneralType));
        return generalResource.generalIcons.Find((a) => a.rairityType == general.RairtyType).generalIcon;
    }
    public Sprite GetClassIcon(General general)
    {
        return classIconSprites.Find((a) => (a.classType == general.classType)).classIcon;
    }
    public Sprite GetRairityIcon(General general)
    {
        return rairityIconSprites.Find((a) => a.rairityType == general.RairtyType).rairityIcon;
    }
    public Sprite GetGeneralPaint(General general)
    {
        return generalResources.Find((a) => a.generalType == general.GeneralType).generalPaint;
    }
    public Sprite GetGeneralCardHead(General general)
    {
        return generalResources.Find((a) => a.generalType == general.GeneralType).generalCardHead;
    }
}
