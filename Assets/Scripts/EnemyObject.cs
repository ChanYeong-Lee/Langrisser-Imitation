using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MovingObject
{
    [SerializeField] private int level;
    [SerializeField] private GeneralType generalType;
    [SerializeField] private SoldierType soldierType;
    
    private void OnEnable()
    {
        identity = IdentityType.Enemy;
        SetGeneral();
        SetSoldier();
        Init();
    }

    public void SetGeneral()
    {
        general = Instantiate(DataManager.Instance.GetGeneral(generalType), transform.position, Quaternion.identity);
        while (general.Level == level)
        {
            general.LevelUp();
        }
        general.transform.parent = this.transform;
    }

    public void SetSoldier()
    {
        soldier = Instantiate(DataManager.Instance.GetSoldier(soldierType), transform.position, Quaternion.identity);
        soldier.transform.parent = this.transform;
    }


}
