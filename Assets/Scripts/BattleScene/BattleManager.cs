using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public enum State { Ready, Battle }
    public State state;
    public enum MoveState { Ready, Move, End }
    public MoveState moveState;
    public UnityEvent onFight;
    public static BattleManager Instance { get; private set; }
    [HideInInspector] public BattleMap currentBattleMap;
    public List<MovingObject> movingObjects;
    public List<AllyObject> allyObjects;
    public List<EnemyObject> enemyObjects;

    public GameObject selectCellArea;

    private MovingObject currentObject;
    public MovingObject CurrentObject { get { return currentObject; } set { currentObject = value; if(value != null) originCell = currentObject.currentCell; onObjectChange?.Invoke();
        } }
    private MovingObject nextObject;
    public MovingObject NextObject { get { return nextObject; } }

    public UnityEvent onObjectChange;

    public BattleMapCell currentCell;
    public BattleMapCell nextCell;
    BattleMapCell originCell = null;
    public bool canSelect;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BattleMapManager.Instance.LoadMap(GameManager.Instance.currentStageID);
        TurnManager.Instance.onTurnChange.AddListener(TurnChange);
        BattleUIManager.Instance.ReadyBattle();
        state = State.Ready;
        canSelect = true;
    }

    private void Update()
    {
        //if (canSelect)
        //{
        //    switch (state)
        //    {
        //        case State.Ready:
        //            SelectCell();
        //            break;
        //        case State.Battle:
        //            if (TurnManager.Instance.TurnState == TurnManager.State.PlayerTurn)
        //            {
        //                switch (moveState)
        //                {
        //                    case MoveState.Ready:
        //                        CharacterAction();
        //                        SelectCell();
        //                        break;
        //                    case MoveState.Move:
        //                        ApplyInput();
        //                        break;
        //                    case MoveState.End:
        //                        break;
        //                }
        //            }
        //            break;
        //    }
        //}

        if (Input.GetMouseButtonUp(0) && canSelect)
        {
            if (SelectCell())
            {
                if (CheckCharacter())
                {

                }
                if (state == State.Ready)
                { }
                if (state == State.Battle)
                {
                    DrawCell();
                }
            }
            else
            {
                UnDrawCell();
            }
        }
    }

    private void DragElement()
    {
    }
    public void StartBattle()
    {
        state = State.Battle;
        List<MovingObject> deadList = new List<MovingObject>();
        foreach (MovingObject movingObject in allyObjects)
        {
            if (movingObject.general == null)
                deadList.Add(movingObject);
        }
        foreach (MovingObject deadObject in deadList)
        {
            allyObjects.Remove(deadObject as AllyObject);
            movingObjects.Remove(deadObject);
            Destroy(deadObject.gameObject);
        }
        TurnManager.Instance.StartBattle();
    }

    private bool SelectCell()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit == false) return false;
        if (hit.collider.TryGetComponent(out BattleMapCell cell))
        {
            if (currentCell == null) { currentCell = cell; }
            nextCell = cell;
            return true;
        }
        else
        {
            nextCell = null;
            return false;
        }
    }

    private void DrawCell()
    {
        if (selectCellArea.activeInHierarchy == false) { selectCellArea.SetActive(true); }
        selectCellArea.transform.position = currentCell.transform.position;
    }

    private void UnDrawCell()
    {
        selectCellArea.SetActive(false);
    }
    private bool CheckCharacter()
    {
        if (nextCell.movingObject != null)
        {
            if (nextCell.movingObject.general == null)
            {
                nextObject = null;
                return false;
            }
            if (CurrentObject == null) { CurrentObject = currentCell.movingObject; }
            nextObject = currentCell.movingObject;
            return true;
        }
        else
        {
            nextObject = null;
            return false;
        }
    }

    private void UpdateCharacter()
    {
        CurrentObject = nextObject;
    }
    private void DrawAreas()
    {
        CurrentObject.DrawAreas();
    }
    private void UnDrawAreas()
    {
        if (CurrentObject != null)
        {
            CurrentObject.UnDrawAreas();
        }
    }
    private void CharacterAction()
    {
        if (CurrentObject != null && CurrentObject.identity == IdentityType.Ally)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit == false) return;
                if (hit.collider.TryGetComponent(out BattleMapCell cell))
                {
                    if (originCell != null) CurrentObject.SetPos(originCell);
                    originCell = CurrentObject.CurrentCell;
                    if (cell.movingObject != null && cell.movingObject.identity == IdentityType.Enemy)
                    {
                        if (CurrentObject.TryAttack(cell))
                        {
                            moveState = MoveState.Move;
                            BattleUIManager.Instance.ReadyAttack();
                        }
                    }
                    else if (CurrentObject.TryMove(cell))
                    {
                        moveState = MoveState.Move;
                        BattleUIManager.Instance.ReadyMove();
                    }
                }
            }
        }
    }

    public void MoveCancel()
    {
        CurrentObject.SetPos(originCell);
        moveState = MoveState.Ready;
        originCell = null;
    }

    public void MoveApply()
    {
        CurrentObject.Move();
        CurrentObject.Attack();
    }

    public bool ApplyInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit == false)
            {
                MoveCancel();
                return false;
            }
            if (hit.collider.TryGetComponent(out BattleMapCell cell))
            {
                if (false == CurrentObject.attackableArea.Contains(cell)||
                    cell.movingObject==null||
                    cell.movingObject.identity == IdentityType.Ally)
                {
                    MoveCancel();
                    return false;
                }

            }
        }
        return false;
    }

    private void TurnChange()
    {
        if (CurrentObject != null)
        {
            CurrentObject.UnDrawAreas();
        }
        if (currentCell != null)
        {
            currentCell = null;
        }
    }

    public void StartFight(MovingObject attacker, MovingObject target)
    {
        int distance = BattleMapAStartAlgorithm.Distance(attacker.currentCell, target.currentCell);
        print(distance);
        if (attacker.range >= distance)
        {
            target.TakeHit(attacker);
        }
        if (target.range >= distance)
        {
            attacker.TakeHit(target);
        }
    }

    public void SetBattleMap(BattleMap battleMap)
    {
        currentBattleMap = battleMap;
        movingObjects = currentBattleMap.movingObjects;
        foreach (MovingObject movingObject in movingObjects)
        {
            movingObject.currentMap = this.currentBattleMap;
            if (movingObject.identity == IdentityType.Ally)
            {
                allyObjects.Add(movingObject as AllyObject);
                movingObject.UpdateCurrentCell();
            }
            else if (movingObject.identity == IdentityType.Enemy)
            {
                enemyObjects.Add(movingObject as EnemyObject);
                movingObject.GetComponent<EnemyObject>().InitEnemyObject();
            }
        }
    }

    public bool CheckAllyDie()
    {
        foreach (MovingObject allyObject in allyObjects)
        {
            if (allyObject.Alive)
                return false;
            else continue;
        }
        return true;
    }
    public bool CheckEnemyDie()
    {
        foreach (MovingObject enemyObject in enemyObjects)
        {
            if (enemyObject.Alive)
                return false;
            else continue;
        }
        return true;
    }
}
