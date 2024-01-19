using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using UnityEngine;

// �����Ȳ�� ���� ��带 Ȱ��ȭ��Ų��.
public class WorldMapData : MonoBehaviour
{
    public WorldMapNode[] nodes;
    public NodeEvent[] nodeEvents;
    public WorldMapNode playerNode;

    public void Init()
    {
        foreach (WorldMapNode node in nodes)
        {
            node.Init();
            if (GameManager.Instance.currentNodeID == node.nodeID)
            {
                playerNode = node;
            }
        }
        foreach (NodeEvent nodeEvent in nodeEvents)
        {
            nodeEvent.Init();
            if (GameManager.Instance.currentEventID == nodeEvent.eventID)
            {
                if (false == GameManager.Instance.stageClear)
                {
                    nodeEvent.ReplaceNode();
                }
            }
        }
    }
}

