using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    [HideInInspector] public BattleMap currentBattleMap;
    public List<AllyObject> allyObjects;
    public List<EnemyObject> enemyObjects;
    private void Awake()
    {
        Instance = this;
    }
    public void TestBattle()
    {
        for (int i = 0; i < allyObjects.Count; i++)
        {
            allyObjects[i].SetGeneral( PlayerData.Instance.generals[i]);
        }
    }

    public void SetBattleMap(BattleMap battleMap)
    {
        currentBattleMap = battleMap;
        foreach (MovingObject movingObject in currentBattleMap.movingObjects)
        {
            if (movingObject.identity == IdentityType.Ally)
            {
                allyObjects.Add(movingObject as AllyObject);
            }
            else if (movingObject.identity == IdentityType.Enemy)
            {
                enemyObjects.Add(movingObject as EnemyObject);
            }
        }
    }


}
