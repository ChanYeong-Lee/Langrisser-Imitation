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
public class Map : MonoBehaviour
{
    public Node[] nodes;
    public Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

}

