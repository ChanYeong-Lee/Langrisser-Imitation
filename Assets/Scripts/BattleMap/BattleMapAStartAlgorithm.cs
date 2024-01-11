using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapAStartAlgorithm : MonoBehaviour
{
    private static Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int( 0, 1),
        new Vector2Int( 0,-1),
        new Vector2Int(-1, 0),
        new Vector2Int( 1, 0)
    };

    private class ASNode
    {
        public BattleMapCell cell;
        public ASNode parent;
        public bool visited = false;
        public int g;
        public int h;
        public int f;
        public ASNode(BattleMapCell cell, ASNode parent, bool visited, int g, int h)
        {
            this.cell = cell;
            this.parent = parent;
            this.visited = visited;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }
    }
    private static int Distance(BattleMapCell start, BattleMapCell end)
    {
        return Mathf.Abs(start.cellcood.x - end.cellcood.x) + Mathf.Abs(start.cellcood.y - end.cellcood.y);
    }

    public static bool PathFinding(BattleMap currentMap, BattleMapCell start, BattleMapCell end, out List<BattleMapCell> path, out int needMoveCost)
    {
        List<ASNode> findNodes = new List<ASNode>();
        List<BattleMapCell> parentList = new List<BattleMapCell>();
        HeapQueue<ASNode, int> heapQueue = new HeapQueue<ASNode, int>();

        ASNode startNode = new ASNode(start, null, false, 0, Distance(start, end));
        findNodes.Add(startNode);
        heapQueue.Enqueue(startNode, startNode.f);

        while (heapQueue.Count > 0)
        {
            ASNode currentNode = heapQueue.Dequeue();
            if (currentNode.visited) continue;
            if (currentNode.cell == end)
            {
                needMoveCost = currentNode.g;
                while (true)
                {
                    parentList.Add(currentNode.cell);
                    currentNode = currentNode.parent;
                    if (currentNode.cell == start) break;
                }
                parentList.Reverse();
                path = parentList;
                return true;
            }

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2Int newPos = currentNode.cell.cellcood + directions[i];

                if (currentMap.cellDictionary.TryGetValue(newPos, out BattleMapCell newCell))
                {
                    if (newCell.moveCost == int.MaxValue) continue;
                    int g = currentNode.g + newCell.moveCost;
                    int h = Distance(newCell, end);
                    ASNode newNode = new ASNode(newCell, currentNode, false, g, h);
                    ASNode findNode = findNodes.Find((a) => a.cell == newCell);
                    if (findNode != null)
                    {
                        if (newNode.f < findNode.f)
                        {
                            findNode = newNode;
                        }
                        else continue;
                    }
                    else
                    {
                        findNodes.Add(newNode);
                    }
                    heapQueue.Enqueue(newNode, newNode.f);
                }
            }
            currentNode.visited = true;
        }
        path = null;
        needMoveCost = int.MaxValue;
        return false;
    }
}
