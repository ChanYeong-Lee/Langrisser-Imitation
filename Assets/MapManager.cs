using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapData mapData;
    public void LoadMap(int stageID)
    {
        BattleMap battleMap = mapData.battleMapDictionary[stageID];

    }
}
