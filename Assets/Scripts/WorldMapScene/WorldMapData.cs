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

// �����Ȳ�� ���� ��带 Ȱ��ȭ��Ų��.
public class WorldMapData
    : MonoBehaviour
{
    public Node[] nodes;
    public WorldMapPlayer player;

    private void Start()
    {
        player = FindObjectOfType<WorldMapPlayer>();
    }

}

