using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        if (nodes.Count == 0) throw new InvalidOperationException();
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