using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapAStartAlgorithm
{
    private class ASNode
    {
        public WorldMapNode node;
        public ASNode parent;
        public bool visited = false;
        public float g;
        public float h;
        public float f;
        public ASNode(WorldMapNode node, ASNode parent, bool visited, float g, float h)
        {
            this.node = node;
            this.parent = parent;
            this.visited = visited;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }
    }
    private static float Distance(WorldMapNode start, WorldMapNode end)
    {
        float distance = Vector3.SqrMagnitude(start.transform.position - end.transform.position);
        return distance;
    }

    public static bool PathFinding(WorldMapNode start, WorldMapNode end, out List<WorldMapNode> path)
    {
        List<ASNode> findNodes = new List<ASNode>();
        List<WorldMapNode> parentList = new List<WorldMapNode>();
        HeapQueue<ASNode, float> heapQueue = new HeapQueue<ASNode, float>();

        ASNode startNode = new ASNode(start, null, false, 0, Distance(start, end));
        findNodes.Add(startNode);
        heapQueue.Enqueue(startNode, startNode.f);

        while (heapQueue.Count > 0)
        {
            ASNode currentNode = heapQueue.Dequeue();
            if (currentNode.visited) continue;
            if (currentNode.node == end)
            {
                while (true)
                {
                    parentList.Add(currentNode.node);
                    currentNode = currentNode.parent;
                    if (currentNode.node == start) break;
                }
                parentList.Reverse();
                path = parentList;
                return true;
            }

            if (currentNode.node.adjNodes.Count > 0)
            {
                foreach (WorldMapNode node in currentNode.node.adjNodes)
                {
                    float g = currentNode.g + Distance(currentNode.node, node);
                    float h = Distance(node, end);
                    ASNode newNode = new ASNode(node, currentNode, false, g, h);
                    if (findNodes.Exists((a) => a.node == node))
                    {
                        ASNode existNode = findNodes.Find((a) => a.node == node);
                        if (newNode.f < existNode.f)
                        {
                            existNode = newNode;
                            heapQueue.Enqueue(newNode, newNode.f);
                        }
                        else continue;
                    }
                    else
                    {
                        findNodes.Add(newNode);
                        heapQueue.Enqueue(newNode, newNode.f);
                    }
                }
            }
        }
        path = null;
        return false;
    }
}