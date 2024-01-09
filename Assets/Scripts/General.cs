using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeneralType
{
    Mathew,
    Grenier,
    Aimeeda,
    GeneralSwordHorse,
    GeneralSwordMan,
    GeneralPikeMan
}

public enum RairityType
{
    N,
    R,
    SR,
    SSR
}

public class General : Character
{
    [SerializeField] private Soldier soldier;
    [SerializeField] private GeneralType generalType;
    [SerializeField] private RairityType rairityType;

    public Soldier Soldier { get { return soldier; } }
    public GeneralType GeneralType { get { return generalType; } }
    public RairityType RairtyType { get { return rairityType; } }

    private int level = 1;
    public int Level { get { return level; } set { level = value; } }

    private int currentExp;
    public int CurrentExp { get { return currentExp; } set { currentExp = value; if (currentExp >= maxExp) { currentExp -= maxExp; LevelUp(); } } }
    private int maxExp;
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
