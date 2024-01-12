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

public class BattleMapCell : MonoBehaviour
{
    public CellType cellType;                       // 타입
    public int moveCost;          // 이동비용
    [HideInInspector] public bool onObject;         // 오브젝트가 위에 있는지 판단
    [HideInInspector] public Vector2Int cellcood;   // 위치
    public MovingObject movingObject;
    public SpriteRenderer spriteRenderer;
    public void InitCell()
    {
        RemoveSprite();
        SetCellCood();
        SetMoveCost();
    }

    private void RemoveSprite()
    {
        spriteRenderer.gameObject.SetActive(false);
    }
    private void SetCellCood()
    {
        cellcood = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
    }

    private void SetMoveCost()
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
