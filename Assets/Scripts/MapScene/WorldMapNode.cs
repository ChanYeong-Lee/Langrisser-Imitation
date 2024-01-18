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
    public void Init()
    {
        gameObject.SetActive(false);
        if (stageID <= GameManager.Instance.stageID)
            gameObject.SetActive(true);
    }

    public NodeType type;
    public int nodeID;
    public int stageID;
    public NodeEvent nodeEvent = null;

    public List<WorldMapNode> adjNodes;
}
