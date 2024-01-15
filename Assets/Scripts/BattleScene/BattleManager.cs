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
    public UnityEvent onFight;
    public static BattleManager Instance { get; private set; }
    [HideInInspector] public BattleMap currentBattleMap;
    public List<MovingObject> movingObjects;
    public List<AllyObject> allyObjects;
    public List<EnemyObject> enemyObjects;

    private MovingObject currentObject;
    public MovingObject CurrentObject { get { return currentObject; } set { currentObject = value; onObjectChange?.Invoke();
        } }
    public UnityEvent onObjectChange;

    public BattleMapCell currentCell;

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
    }

    private void Update()
    {
        switch (state)
        {
            case State.Ready:
                break;
            case State.Battle:
                if (TurnManager.Instance.TurnState == TurnManager.State.PlayerTurn)
                {
                    CharacterAttack();
                    CharacterMove();
                }
                break;
        }
        SelectCell();
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

    private void SelectCell()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit == false) return;
            if (hit.collider.TryGetComponent(out BattleMapCell cell))
            {
                if (CurrentObject != null)
                {
                    CurrentObject.UnDrawAreas();
                }
                currentCell = cell;
                CurrentObject = null;
                CheckCharacter();
            }
        }
    }
    private void CheckCharacter()
    {
        if (currentCell.movingObject != null)
        {
            CurrentObject = currentCell.movingObject;
            CurrentObject.DrawAreas();
        }
    }

    private void CharacterMove()
    {
        if (CurrentObject != null && CurrentObject.identity == IdentityType.Ally)
        {
            if (Input.GetMouseButtonUp(0))
            {
                print("GGOODD");
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit == false) return;
                if (hit.collider.TryGetComponent(out BattleMapCell cell))
                {
                    CurrentObject.TryMove(cell);
                }
            }
        }
    }
    private void CharacterAttack()
    {
        if (CurrentObject != null && CurrentObject.identity == IdentityType.Ally)
        {
            if (Input.GetMouseButtonUp(0))
            {
                print("GGOODD");
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit == false) return;
                if (hit.collider.TryGetComponent(out BattleMapCell cell))
                {
                    if (cell.movingObject != null && cell.movingObject.identity == IdentityType.Enemy) 
                        CurrentObject.TryAttack(cell);
                }
            }
        }
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
