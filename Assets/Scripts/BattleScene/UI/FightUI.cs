using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum FightType
{
    Attacker,
    Target
}

public class FightUI : MonoBehaviour
{
    private MovingObject ally;
    private MovingObject enemy;

    public FightUIElement allyInstructionUI;
    public FightUIElement enemyInstructionUI;

    public void StartFight(MovingObject attacker, MovingObject target)
    {
        if (attacker.identity == IdentityType.Ally)
        {
            ally = attacker;
            enemy = target;
            allyInstructionUI.SetElement(attacker, FightType.Attacker);
            enemyInstructionUI.SetElement(target, FightType.Target);
        }
        else
        {
            ally = target;
            enemy = attacker;
            allyInstructionUI.SetElement(target, FightType.Target);
            enemyInstructionUI.SetElement(attacker, FightType.Attacker);
        }

        int distance = BattleMapAStartAlgorithm.Distance(attacker.currentCell, target.currentCell);
       
        float allyDamage = ally.general.damage;
        if (ally.soldier.Alive) allyDamage += ally.soldier.damage;
        float enemyDamage = enemy.general.damage;
        if (enemy.soldier.Alive) enemyDamage += enemy.soldier.damage;
        if (ally.range < distance) allyDamage = 0;
        if (enemy.range < distance) enemyDamage = 0; 
        allyInstructionUI.UpdateElement(enemyDamage);
        enemyInstructionUI.UpdateElement(allyDamage);
    }
}
