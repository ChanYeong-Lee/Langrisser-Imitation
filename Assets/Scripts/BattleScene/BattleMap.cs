using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMap : MonoBehaviour
{
    [HideInInspector] public Vector2 mapMinBounds;
    [HideInInspector] public Vector2 mapMaxBounds;

    [SerializeField] private int stageID;
    public int StageID { get { return stageID; } }  
    [SerializeField] private SpriteRenderer mapImage;
    [SerializeField] private List<BattleMapCell> cellList;
    public List<MovingObject> movingObjects;
    public Transform areaParent;
    public Dictionary<Vector2Int, BattleMapCell> cellDictionary = new();
    public int dropExp;
    public BattleMapCell this[Vector2Int cood] 
    {
        get 
        {
            if (cellDictionary.TryGetValue(cood, out BattleMapCell cell))
            {
                return cell;
            }
            else 
            {
                return null; 
            }
        }
    }

    public void InitBattleMap()
    {
        CalculateBounds();
        SetDictionary();
    }

    private void CalculateBounds()
    {
        mapMinBounds.x = transform.position.x - mapImage.bounds.extents.x + mapImage.transform.localPosition.x;
        mapMinBounds.y = transform.position.y - mapImage.bounds.extents.y + mapImage.transform.localPosition.y;
        mapMaxBounds.x = transform.position.x + mapImage.bounds.extents.x + mapImage.transform.localPosition.x;
        mapMaxBounds.y = transform.position.y + mapImage.bounds.extents.y + mapImage.transform.localPosition.y;
    }

    private void SetDictionary()
    {
        foreach (BattleMapCell cell in cellList)
        {
            cell.InitCell();
            cellDictionary.Add(cell.cellcood, cell);
        }
    }
}
