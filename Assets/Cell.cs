using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Obstacle = 0,
    Normal = 1,
    Water = 2,
    Tree = 3,
    Rock = 4
}

public class Cell : MonoBehaviour
{
    public CellType cellType;
    public int moveCost;

    [HideInInspector] public Vector2Int cellcood;

    private void Start()
    {
        cellcood = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        CalculateMoveCost();
    }

    private void CalculateMoveCost()
    {
        switch (cellType)
        {
            case CellType.Obstacle:
                moveCost = int.MaxValue;
                break;
            case CellType.Normal:
                moveCost = 1;
                break;
            case CellType.Water:
            case CellType.Tree:
            case CellType.Rock:
                moveCost = 2;
                break;
        }
    }
}
