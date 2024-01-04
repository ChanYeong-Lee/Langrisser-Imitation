using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Unity.VisualScripting;
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
                while (currentNode.parent != null)
                {
                    parentList.Add(currentNode.node);
                    currentNode = currentNode.parent;
                }
                parentList.Reverse();
                path = parentList;
                return true;
            }

            if (currentNode.node.nodes.Count > 0)
            {
                foreach (Node node in currentNode.node.nodes)
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
                    if (PathFinding(player.currentNode, node, out List<Node> path))
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
