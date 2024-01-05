using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public List<BattleMap> battleMapList;
    public Dictionary<int, BattleMap> battleMapDictionary = new Dictionary<int, BattleMap>();
    public Dictionary<int, int[,]> battleMapDataDictionary = new Dictionary<int, int[,]>();
    private void Start()
    {
        foreach (BattleMap battleMap in battleMapList)
        {
            battleMapDictionary[battleMap.stageID] = battleMap;
        }
    }

}
