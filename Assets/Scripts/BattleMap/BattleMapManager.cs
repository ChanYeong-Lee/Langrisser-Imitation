using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapManager : MonoBehaviour
{
    public static BattleMapManager Instance { get; private set; }
    [SerializeField] private BattleMapData mapData;
    [SerializeField] new private BattleSceneCameraMove camera;
    private BattleMap currentBattleMap;
    private void Awake()
    {
        Instance = this;
    }
    public void LoadMap(int stageID)
    {
        currentBattleMap = Instantiate(mapData.battleMapDictionary[stageID]);
        currentBattleMap.InitBattleMap();
        camera.SetBattleMap(currentBattleMap);
        BattleManager.Instance.SetBattleMap(currentBattleMap);
    }
}
