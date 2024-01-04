using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Node[] nodes;
    public Player player;
    private class PNode
    {
        public Node node;
        public PNode parent = null;
        public bool visited;
        public PNode(Node node, PNode parent, bool visited)
        {
            this.node = node;
            this.parent = parent;
            this.visited = visited; 
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        CheckNode();
    }

    public bool PathFinding(Node start, Node end, out List<Node> path)
    {
        PNode currentNode = new PNode(start, null, false);
        Queue<PNode> nodeQueue = new Queue<PNode>();
        List<PNode> findList = new List<PNode>();
        List<Node> parentList = new List<Node>();

        nodeQueue.Enqueue(currentNode);
        findList.Add(currentNode);
        while (nodeQueue.Count > 0)
        {
            currentNode = nodeQueue.Dequeue();

            if (currentNode.visited)
                continue;
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
                foreach (Node node in currentNode.node.adjNodes)
                {
                    if (findList.Exists((a) => a.node == node))
                    {
                        continue;
                    }
                    else
                    {
                        PNode newPNode = new PNode(node, currentNode, false);
                        nodeQueue.Enqueue(newPNode);
                        findList.Add(newPNode);
                    }
                }
            }
            currentNode.visited = true;
        }
        path = null;
        return false;
    }

    private void CheckNode()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (ClickHitInfo(out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent<Node>(out Node node))
                {
                    print("You Click Node");
                    if (AStarAlgorithm.PathFinding(player.currentNode, node, out List<Node> path))
                    {
                        if (path.Count > 0)
                        {
                            player.Move(path);
                        }
                    }
                    else
                    {
                        print("Can't Reach");
                    }
                }
            }
        }
    }
    private bool ClickHitInfo(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            hit = hitInfo;
            return true;
        }
        else
        {
            hit = default(RaycastHit);
            return false;
        }
    }
}

public class AStarAlgorithm
{
    private class ASNode
    {
        public Node node;
        public ASNode parent;
        public bool visited = false;
        public float g;
        public float h;
        public float f;
        public ASNode(Node node, ASNode parent, bool visited, float g, float h)
        {
            this.node = node;
            this.parent = parent;
            this.visited = visited;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }
    }
    private static float Distance(Node start, Node end)
    {
        float distance = Vector3.SqrMagnitude(start.transform.position - end.transform.position);
        return distance;
    }

    public static bool PathFinding(Node start, Node end, out List<Node> path)
    {
        List<ASNode> findNodes = new List<ASNode>();
        List<Node> parentList = new List<Node>();   
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
                foreach (Node node in currentNode.node.adjNodes)
                {
                    float g = currentNode.g + Distance(currentNode.node, node);
                    float h = Distance(node, end);
                    ASNode newNode = new ASNode(node, currentNode, false, g, h);
                    if (findNodes.Exists((a) => a.node == node))
                    {
                        ASNode existNode = findNodes.Find((a) => a.node == node);
                        if (newNode.f < existNode.f) heapQueue.Enqueue(newNode, newNode.f);
                        else continue;
                    }
                    else heapQueue.Enqueue(newNode, newNode.f);
                }
            }
        }
        path = null;
        return false;
    }


}

public class HeapQueue<TItem, TPriority>
{
    private struct Node
    {
        public TItem item;
        public TPriority priority;
        public Node(TItem item, TPriority priority) 
        {
            this.item = item; this.priority = priority; 
        }
    }
    private List<Node> nodes = new List<Node>();
    private IComparer<TPriority> comparer = Comparer<TPriority>.Default;

    public int Count => nodes.Count;
    public TItem Peek()
    {
        if(nodes.Count == 0) throw new InvalidOperationException();
        return nodes[0].item;
    }

    public void Enqueue(TItem item, TPriority priority)
    {
        Node newNode = new Node(item, priority);
        PushHeap(newNode);
    }

    public TItem Dequeue()
    {
        if (nodes.Count == 0) throw new InvalidOperationException();
        return PopHeap().item;
    }
    
    private void PushHeap(Node node)
    {
        nodes.Add(node);
        int index = nodes.Count - 1;
        while (index > 0)
        {
            int parentIndex = GetParentIndex(index);
            Node parentNode = nodes[parentIndex];
            if (comparer.Compare(node.priority, parentNode.priority) < 0)
            {
                nodes[index] = parentNode;
                index = parentIndex;
            }
            else break;
        }
        nodes[index] = node;
    }

    private Node PopHeap()
    {
        Node rootNode = nodes[0];
        ArrangeHeap();
        return rootNode;
    }

    private void ArrangeHeap()
    {
        Node lastNode = nodes[nodes.Count - 1];
        nodes.RemoveAt(nodes.Count - 1);
        int index = 0;
        while (index < nodes.Count)
        {
            int leftChildIndex = GetLeftChildIndex(index);
            int rightChildIndex = GetRightChildIndex(index);

            if (rightChildIndex < nodes.Count)
            {
                int compareIndex =
                    comparer.Compare(nodes[leftChildIndex].priority, nodes[rightChildIndex].priority) < 0 ?
                    leftChildIndex : rightChildIndex;

                if (comparer.Compare(nodes[compareIndex].priority, lastNode.priority) < 0)
                {
                    nodes[index] = nodes[compareIndex];
                    index = compareIndex;
                }
                else { nodes[index] = lastNode; break; }
            }
            else if (leftChildIndex < nodes.Count)
            {
                if (comparer.Compare(nodes[leftChildIndex].priority, lastNode.priority) < 0)
                {
                    nodes[index] = nodes[leftChildIndex];
                    index = leftChildIndex;
                }
                else { nodes[index] = lastNode; break; }
            }
            else { nodes[index] = lastNode; break; }
        }
    }

    private int GetParentIndex(int index) => (index - 1) / 2;
    private int GetLeftChildIndex(int index) => index * 2 + 1;
    private int GetRightChildIndex(int index) => index * 2 + 2;
}