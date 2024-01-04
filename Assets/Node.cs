using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public enum NodeType
    {
        Road,
        Object
    }

    public NodeType type;
    public bool somethingHere = false;

    public INodeEvent nodeEvent = null;

    public List<Node> nodes;
}
