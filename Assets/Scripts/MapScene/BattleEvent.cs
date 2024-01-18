using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent : NodeEvent
{
    public override void Execute()
    {
        GameManager.Instance.EnterBattle(this);
    }
}
