using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public List<BattleMapCell> attackableArea;
    private List<GameObject> activeArea = new List<GameObject>();
    public BattleMapCell prevCell;
    public BattleMapCell currentCell;

    public BattleMap currentMap;
    public MovingEngine engine;
    public GameObject movableAreaPrefab;
    public GameObject attackableAreaPrefab;
    public Transform generalPos;
    public Transform soldierPos;
    public Transform areaParent;
    public SpriteRenderer spriteRenderer;

    public bool isMoving;
    public bool canMove;
    public bool canAttack;

    [HideInInspector] public Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int( 0,  1),     // ��
        new Vector2Int( 0, -1),     // ��
        new Vector2Int( 1,  0),     // ��
        new Vector2Int(-1,  0),     // ��
    };

    protected virtual void Awake()
    {
        engine = GetComponent<MovingEngine>();
        spriteRenderer.gameObject.SetActive(false);
    }

    protected void InitMovingObject()
    {
        SetStats();
        SetPositions();
        general.OnDie.AddListener(CheckAlive);
        soldier.OnDie.AddListener(CheckAlive);
        RestoreCosts();
        UpdateCurrentCell();
    }
    protected void Release()
    {
        general.OnDie.RemoveAllListeners();
        soldier.OnDie.RemoveAllListeners();
    }

    private void SetStats()
    {
        int biggerRange = general.range >= soldier.range ? general.range : soldier.range;
        int smallerMoveCost = general.moveCost > soldier.moveCost ? soldier.moveCost : general.moveCost;
        range = biggerRange;
        moveCost = smallerMoveCost;
    }
    private void SetPositions()
    {
        general.transform.parent = generalPos;
        general.transform.position = generalPos.position;
        soldier.transform.parent = soldierPos;
        soldier.transform.position = soldierPos.position;
    }
    public void RestoreCosts()
    {
        currentMoveCost = moveCost;
        canMove = true;
        canAttack = true;
    }
    public void UpdateCurrentCell()
    {
        if (prevCell != null)
        {
            prevCell.movingObject = null;
        }
        currentCell = currentMap.cellDictionary[Vector2Int.RoundToInt(transform.localPosition)];
        currentCell.movingObject = this;
        prevCell = currentCell;
        CheckAreas();
    }

    public void CheckAreas()
    {
        CheckMovableArea();
        CheckAttackableArea();
    }

    public void CheckMovableArea()
    {
        movableArea.Clear();
        movableArea = engine.CheckMovableArea();
    }
    public void CheckAttackableArea()
    {
        attackableArea.Clear();
        attackableArea = engine.CheckAttackableArea();
    }
    public void DrawAreas()
    {
        DrawMovableArea();
        DrawAttackableArea();
    }

    public void DrawMovableArea()
    {
        foreach (BattleMapCell cell in movableArea)
        {
            GameObject area = Instantiate(movableAreaPrefab, cell.transform.position, Quaternion.identity);
            area.transform.parent = areaParent;
            activeArea.Add(area);
        }
    }
    public void DrawAttackableArea()
    {
        foreach (BattleMapCell cell in attackableArea)
        {
            if (movableArea.Exists((a) => a.cellcood == cell.cellcood)) continue;

            GameObject area = Instantiate(attackableAreaPrefab, cell.transform.position, Quaternion.identity);
            area.transform.parent = areaParent;
            activeArea.Add(area);
        }
    }
    public void UnDrawAreas()
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
        DrawAreas();
    }
    private void CheckAlive()
    {
        if (false == general.Alive && false == soldier.Alive)
        {
            alive = false;
            OnDie?.Invoke(this);
        }
    }

    public bool TryMove(BattleMapCell destination)
    {
        if (canMove)
            return engine.TryMove(destination);
        else return false;
    }

    public bool TryAttack(BattleMapCell destination)
    {
        if (canAttack)
            return engine.TryAttack(destination);
        else return false;
    }

    public void TakeHit(MovingObject attacker)
    {
        print($"{attacker.general.gameObject.name} hit {gameObject.name}");
        float damage = attacker.general.damage;
        float remainDamage = 0;
        if (attacker.soldier.Alive) damage += attacker.soldier.damage;
        if (soldier.Alive) remainDamage = soldier.TakeHit(damage);
        general.TakeHit(remainDamage);
    }
}
