using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMap : MonoBehaviour
{
    public int stageID;
    public SpriteRenderer mapImage;
    public List<Cell> cellList;
    [HideInInspector] public Vector2 mapMinBounds;
    [HideInInspector] public Vector2 mapMaxBounds;

    public Dictionary<Vector2Int, Cell> cellDictionary = new();

    private void Start()
    {
        mapMinBounds.x = transform.position.x - mapImage.bounds.extents.x + mapImage.transform.localPosition.x;
        mapMinBounds.y = transform.position.y - mapImage.bounds.extents.y + mapImage.transform.localPosition.y;
        mapMaxBounds.x = transform.position.x + mapImage.bounds.extents.x + mapImage.transform.localPosition.x;
        mapMaxBounds.y = transform.position.y + mapImage.bounds.extents.y + mapImage.transform.localPosition.y;

        foreach (Cell cell in cellList)
        {
            cellDictionary.Add(cell.cellcood, cell);
        }
    }
}
