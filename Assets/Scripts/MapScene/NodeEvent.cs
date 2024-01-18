using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public abstract class NodeEvent : MonoBehaviour
{
    public int eventID;
    public int stageID;
    public WorldMapNode node;
    public void Init()
    {
        gameObject.SetActive(false);
        if (stageID <= GameManager.Instance.stageID)
        {
            gameObject.SetActive(true);
        }
        if (GameManager.Instance.eventDictionary.TryGetValue(eventID, out bool clear)) 
        {
            gameObject.SetActive(!clear);
        }
    }

    public void RemoveNode()
    {
        gameObject.SetActive(false);
    }
    public void ReplaceNode()
    {
        foreach (WorldMapNode node in node.adjNodes)
        {
            if (node.gameObject.activeSelf && node.nodeEvent == null)
            {
                this.node = node;
                node.nodeEvent = this;
                transform.position = node.transform.position;
                break;
            }
        }
    }

    public abstract void Execute();
}
