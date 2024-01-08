using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent : MonoBehaviour, INodeEvent
{
    [SerializeField] private int stageID;

    public void Execute()
    {
        GameManager.Instance.EnterBattle(stageID);
    }
}
