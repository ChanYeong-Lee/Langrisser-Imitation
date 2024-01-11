using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapManager : MonoBehaviour
{
    [SerializeField] private BattleMapData mapData;
    [SerializeField] new private BattleSceneCameraMove camera;
    private BattleMap currentBattleMap;
    private void Start()
    {
        LoadMap(GameManager.Instance.currentStageID);
    }
    public void LoadMap(int stageID)
    {
        currentBattleMap = mapData.battleMapDictionary[stageID];
        currentBattleMap.gameObject.SetActive(true);
        camera.SetBattleMap(currentBattleMap);
        BattleManager.Instance.SetBattleMap(currentBattleMap);
    }
}
