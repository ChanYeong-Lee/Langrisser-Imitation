using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReadyUI : MonoBehaviour
{
    public GeneralInstruction allyInstruction;
    public GeneralInstruction enemyInstruction;

    public RectTransform allyParent;
    public RectTransform enemyParent;

    public void Init()
    {
        allyInstruction.SetParent(allyParent);
        enemyInstruction.SetParent(enemyParent);
    }

    public void SetInstructions(MovingObject ally, MovingObject enemy)
    {
        allyInstruction.SetInstruction(ally);
        enemyInstruction.SetInstruction(enemy);
    }
}
