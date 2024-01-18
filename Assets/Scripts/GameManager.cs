using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { if(instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>(); return instance; } }
    public int currentStageID = 0;
    public int stageID = 0;
    public int currentNodeID;
    public int currentEventID;
    public bool stageClear;

    public Dictionary<int, bool> eventDictionary = new Dictionary<int, bool>();
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EnterBattle(NodeEvent nodeEvent)
    {
        if (false == eventDictionary.ContainsKey(nodeEvent.eventID))
            eventDictionary.Add(nodeEvent.eventID, false);
        currentStageID = nodeEvent.stageID;
        currentEventID = nodeEvent.eventID;
        currentNodeID = nodeEvent.node.nodeID;
        SceneLoader.Instance.LoadScene("BattleScene");
    }

    public void StageClear()
    {
        if (stageID <= currentStageID) { stageID += 1; }
        eventDictionary[currentEventID] = true;
    }
}
