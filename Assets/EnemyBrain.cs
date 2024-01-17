using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public static EnemyBrain Instance { get; private set; }
    public List<MovingObject> allyObjects;
    public List<MovingObject> enemyObjects;
    private MovingObject currentObject;
    private MovingObject currentTarget;
    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        allyObjects = BattleManager.Instance.enemyObjects;
        enemyObjects = BattleManager.Instance.allyObjects;
    }

    public void StartTurn()
    {
        StartCoroutine(BattleCoroutine());
    }
    private bool FindTarget()
    {
        HeapQueue<MovingObject, int> heapQueue = new HeapQueue<MovingObject, int>();
        foreach (MovingObject enemy in enemyObjects)
        {
            if (false == enemy.Alive) continue;
            if (BattleMapAStartAlgorithm.PathFinding(BattleManager.Instance.currentBattleMap, currentObject.currentCell, enemy.currentCell, out List<BattleMapCell> path, out int distance))
            {
                heapQueue.Enqueue(enemy, distance);
            }
        }
        if (heapQueue.Count > 0) 
        {
            currentTarget = heapQueue.Dequeue(out int distance); 
            return true; 
        }
        else return false;
    }

    private bool TryAttack()
    {
        return currentObject.TryAttack(currentTarget.currentCell);
    }

    private void Attack()
    {
        currentObject.Attack();
    }
    private bool TryMove()
    {
        BattleMapAStartAlgorithm.PathFinding(BattleManager.Instance.currentBattleMap, currentObject.currentCell, currentTarget.currentCell, out List<BattleMapCell> path, out int distance);
        path.Reverse();
        foreach (BattleMapCell cell in path)
        {
            if (currentObject.TryMove(cell))
                return true;
        }
        return false;
    }
    private void Move()
    {
        currentObject.Move();
    }
    private void EndTurn()
    {
        TurnManager.Instance.EnemyTurnEnd();
    }
    IEnumerator BattleCoroutine()
    {
        foreach (MovingObject movingObject in allyObjects)
        {
            currentObject = movingObject;
            if (FindTarget())
            {
                if (TryAttack())
                {
                    Attack();
                }
                else if (TryMove())
                {
                    Move();
                }
            }
            else Move();
            yield return new WaitWhile(() => currentObject.isAction);
        }
        EndTurn();
    }
}
