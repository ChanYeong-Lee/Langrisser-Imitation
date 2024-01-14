using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum GeneralType
{
    Mathew,
    Grenier,
    Aimeeda,
    Skot,
    GeneralSwordHorse,
    GeneralSwordMan,
    GeneralPikeMan
}

public enum RairityType
{
    R,
    SR,
    SSR
}

public class General : Character
{
    [SerializeField] private Soldier soldier;
    [SerializeField] private GeneralType generalType;
    [SerializeField] private RairityType rairityType;
    [HideInInspector] public UnityEvent OnExpChange;

    public Soldier Soldier { get { return soldier; } }
    public GeneralType GeneralType { get { return generalType; } }
    public RairityType RairityType { get { return rairityType; } }

    private int level = 1;
    public int Level { get { return level; } set { level = value; } }

    private int currentExp;
    public int CurrentExp { get { return currentExp; } set { currentExp = value; if (currentExp >= maxExp) { currentExp -= maxExp; LevelUp(); } OnExpChange?.Invoke(); } }

    private int maxExp = 100;
    public int MaxExp { get { return maxExp; } }

    public void LevelUp()
    {
        Level++;
        StatUp();
        CurrentHP = maxHP;
    }

    private void StatUp()
    {
        maxHP += 50;
        damage += 10;
        defense += 5;
    }
}
