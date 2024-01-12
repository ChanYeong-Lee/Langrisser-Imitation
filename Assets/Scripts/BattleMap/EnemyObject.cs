using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MovingObject
{
    [SerializeField] private int level;
    [SerializeField] private GeneralType generalType;
    [SerializeField] private SoldierType soldierType;

    protected override void Awake()
    {
        base.Awake();
        identity = IdentityType.Enemy;
    }

    public void InitEnemyObject()
    {
        SetGeneral();
        SetSoldier();
        InitMovingObject();
    }

    public void SetGeneral()
    {
        general = Instantiate(DataManager.Instance.GetGeneral(generalType));
        while (general.Level == level)
        {
            general.LevelUp();
        }
    }

    public void SetSoldier()
    {
        soldier = Instantiate(DataManager.Instance.GetSoldier(soldierType));
    }


}
