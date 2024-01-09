using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum IdentityType
{
    Ally,
    Enemy
}

public enum Direction
{
    Up      = 0,
    Down    = 1,
    Left    = 2,
    Right   = 3
}

public class MovingObject : MonoBehaviour
{
    
    [HideInInspector] public IdentityType identity;
    [HideInInspector] public UnityEvent<MovingObject> OnDie;

    [HideInInspector] public General general;
    [HideInInspector] public Soldier soldier;
    [HideInInspector] public int range;
    [HideInInspector] public int moveCost;
    public int currentMoveCost;
    private bool alive;
    [HideInInspector] public bool Alive { get { return alive; } set { alive = value; if (alive) { general.Alive = true; soldier.Alive = true; } } }
    public List<BattleMapCell> movableArea;
    [HideInInspector] public List<BattleMapCell> attackableArea;
    private List<GameObject> activeArea = new List<GameObject>();
    public BattleMapCell currentCell;

    public BattleMap currentMap;
    public MovingEngine engine;
    public GameObject areaPrefab;
    public Transform generalPos;
    public Transform soldierPos;
    public Transform areaParent;

    [HideInInspector] public Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int( 0,  1),     // ╩С
        new Vector2Int( 0, -1),     // го
        new Vector2Int( 1,  0),     // аб
        new Vector2Int(-1,  0),     // ©Л
    };

    private void OnEnable()
    {
        engine = GetComponent<MovingEngine>();
    }

    protected void Init()
    {
        int biggerRange = general.range >= soldier.range ? general.range : soldier.range;
        int smallerMoveCost = general.moveCost > soldier.moveCost ? soldier.moveCost : general.moveCost;

        general.transform.parent = generalPos;
        general.transform.position = generalPos.position; 
        soldier.transform.parent = soldierPos;
        soldier.transform.position = soldierPos.position;
        general.OnDie.AddListener(CheckAlive);
        soldier.OnDie.AddListener(CheckAlive);
        range = biggerRange;
        moveCost = smallerMoveCost;
    }

    private void UpdateCurrentCell()
    {
        currentCell = currentMap.cellDictionary[Vector2Int.RoundToInt(transform.localPosition)];
    }

    public void CheckMovableArea()
    {
        UpdateCurrentCell();
        movableArea = engine.CheckMovableArea();
    }
    public void DrawMovableArea()
    {
        foreach (BattleMapCell cell in movableArea)
        {
            GameObject area = Instantiate(areaPrefab, cell.transform.position, Quaternion.identity);
            area.transform.parent = areaParent;
            activeArea.Add(area);
        }
    }
    public void UnDrawMovableArea()
    {
        foreach (GameObject area in activeArea)
        {
            Destroy(area);
        }
        activeArea.Clear();
    }
    public void TestMovableArea()
    {
        UpdateCurrentCell();
        CheckMovableArea();
        DrawMovableArea();
    }
    private void CheckAlive()
    {
        if (false == general.Alive && false == soldier.Alive)
        {
            alive = false;
            OnDie?.Invoke(this);
        }
    }
}
