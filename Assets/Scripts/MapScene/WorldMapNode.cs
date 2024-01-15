using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapNode : MonoBehaviour
{
    public enum NodeType
    {
        Road,
        Object
    }

    public NodeType type;
    public bool somethingHere = false;

    public INodeEvent nodeEvent = null;

    public List<WorldMapNode> adjNodes;
}
