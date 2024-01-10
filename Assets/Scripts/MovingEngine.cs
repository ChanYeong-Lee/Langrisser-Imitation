using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingEngine : MonoBehaviour
{
    [HideInInspector] public MovingObject movingObject;
    private void Awake()
    {
        movingObject = GetComponent<MovingObject>();
    }

    private class MovingCell
    {
        public BattleMapCell cell;
        public MovingCell parent;
        public int usingCost;
        public int remainCost;
        public bool visited;
        public MovingCell(BattleMapCell cell, MovingCell parent, int usingCost, int remainCost, bool visited)
        {
            this.cell = cell;
            this.parent = parent;
            this.usingCost = usingCost;
            this.remainCost = remainCost;
            this.visited = visited;
        }
    }

    public List<BattleMapCell> CheckMovableArea()
    {
        BattleMap currentMap = movingObject.currentMap;
        BattleMapCell currentCell = movingObject.currentCell;
        int moveCost = movingObject.moveCost;

        List<MovingCell> movings = new List<MovingCell>();
        Queue<MovingCell> queue = new Queue<MovingCell>();
        MovingCell start = new MovingCell(currentCell, null, 0, moveCost, false);
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            MovingCell cell = queue.Dequeue();
            if (cell.visited) continue;
            for (int i = 0; i < movingObject.directions.Length; i++)
            {
                Vector2Int newCellPos = cell.cell.cellcood + movingObject.directions[i];
                if (false == currentMap.cellDictionary.TryGetValue(newCellPos, out BattleMapCell newBattleMapCell)) continue;
                int usingCost = cell.usingCost + newBattleMapCell.moveCost;
                int remainCost = moveCost - usingCost;
                if (remainCost < 0) continue;
                MovingCell newCell = new MovingCell(newBattleMapCell, cell, usingCost, remainCost, false);
                queue.Enqueue(newCell);
                movings.Add(newCell);
            }
            cell.visited = true;
        }
        List<BattleMapCell> movableArea = new List<BattleMapCell>();
        foreach (MovingCell cell in movings)
        {
            if (movableArea.Exists((a) => a == cell.cell))
                continue;
            movableArea.Add(cell.cell);
        }
        return movableArea;
    }
    public List<BattleMapCell> CheckAttackableArea()
    {
        BattleMap currentMap = movingObject.currentMap;
        List<BattleMapCell> attackableArea = new List<BattleMapCell>();
        List<BattleMapCell> movableArea = movingObject.movableArea;
        foreach (BattleMapCell cell in movableArea)
        {
            attackableArea.Add(cell);
            foreach (Vector2Int newPos in CellArea(cell.cellcood, movingObject.range))
            {
                if (currentMap.cellDictionary.TryGetValue(newPos, out BattleMapCell newCell))
                {
                    if (attackableArea.Contains(newCell)) continue;
                    else attackableArea.Add(newCell);
                }
            }
        }
        return attackableArea;
    }

    private List<Vector2Int> CellArea(Vector2Int centerCell, int range)
    {
        List<Vector2Int> cellArea = new List<Vector2Int>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= range)
                {
                    Vector2Int newPos = new Vector2Int(centerCell.x + x, centerCell.y + y);
                    cellArea.Add(newPos);
                }
            }
        }
        return cellArea;
    }
}
