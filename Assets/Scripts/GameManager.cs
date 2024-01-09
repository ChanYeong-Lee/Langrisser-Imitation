using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { if(instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>(); return instance; } }
    public int currentStageID;
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

    public void EnterBattle(int stageID)
    {
        currentStageID = stageID;
        SceneLoader.Instance.LoadScene("BattleScene");
    }
}
