using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance { get; private set; }
    public GameObject battleReadyUI;
    public GeneralInstruction generalInstruction;
    private void Awake()
    {
        Instance = this;
    }

    public void ReadyBattle()
    {
        battleReadyUI.SetActive(true);
        generalInstruction.Init();
    }

    public void StartBattle()
    {
        bool isReady = false;
        foreach (MovingObject movingObject in BattleManager.Instance.allyObjects)
        {
            if (movingObject.general != null)
            {
                isReady = true;
                break;
            }
        }
        if (isReady)
        {
            battleReadyUI.SetActive(false);
            BattleManager.Instance.StartBattle();
        }
    }
}
