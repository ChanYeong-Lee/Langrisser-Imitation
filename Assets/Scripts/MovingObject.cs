using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IdentityType
{
    Ally,
    Enemy
}

public class MovingObject : MonoBehaviour
{
    [HideInInspector] public IdentityType identity;

    [HideInInspector] public General general;
    [HideInInspector] public Soldier soldier;
    [HideInInspector] public int range;
    [HideInInspector] public int moveCost;
    [HideInInspector] public bool alive;

    [HideInInspector] public List<BattleMapCell> movableArea;
    [HideInInspector] public List<BattleMapCell> attackableArea;
    [HideInInspector] public BattleMapCell currentCell;
    private void Update()
    {
    }
    protected void Init()
    {
        int biggerRange = general.range >= soldier.range ? general.range : soldier.range;
        int smallerMoveCost = general.moveCost > soldier.moveCost ? soldier.moveCost : general.moveCost;

        range = biggerRange;
        moveCost = smallerMoveCost;
        alive = true;
    }
}
