using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapData : MonoBehaviour
{
    [SerializeField] private List<BattleMap> battleMapList;
    public Dictionary<int, BattleMap> battleMapDictionary = new Dictionary<int, BattleMap>();

    private void Awake()
    {
        foreach (BattleMap battleMap in battleMapList)
        {
            battleMapDictionary[battleMap.StageID] = battleMap;
        }
    }
}
