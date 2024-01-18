using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapManager : MonoBehaviour
{
    public static WorldMapManager Instance { get; private set; }
    public WorldMapPlayer player;
    public WorldMapController controller;
    public WorldMapData worldMapData;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        worldMapData.Init();
        player.UpdateNode(worldMapData.playerNode);
        player.Init();
        controller.SetPlayer(player);
        controller.Init();
    }
}
