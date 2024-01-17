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
    public List<MovingObject> allyObjects;
    public List<MovingObject> enemyObjects;

    public GameObject selectCellArea;

    [SerializeField] private MovingObject currentObject;
    public MovingObject CurrentObject { get { return currentObject; }
        set 
        { 
            currentObject = value;
            onObjectChange?.Invoke(); 
        }
    }
    [SerializeField] private MovingObject nextObject;
    public MovingObject NextObject { get { return nextObject; } }

    public UnityEvent onObjectChange;

    public BattleMapCell currentCell;
    public BattleMapCell nextCell;
    public BattleMapCell originCell = null;
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
        EnemyBrain.Instance.Init();
        state = State.Ready;
        canSelect = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && canSelect)
        {
            switch (state)
            {
                case State.Ready:
                    SelectCell();
                    CheckCharacter();
                    CheckRemoveCharacter();
                    UnDrawAreas();
                    UpdateCell();
                    UpdateCharacter();
                    DrawAreas();
                    break;
                case State.Battle:
                    switch (moveState)
                    {
                        case MoveState.Ready:
                            SelectCell();
                            CheckCharacter();
                            if (CharacterAction())
                            {
                                moveState = MoveState.Move;
                                return;
                            }
                            UnDrawAreas();
                            UpdateCell();
                            UpdateCharacter();
                            DrawAreas();
                            break;
                        case MoveState.Move:
                            SelectCell();
                            CheckCharacter();
                            CheckMoveCancel();
                            break;
                        case MoveState.End:
                            break;
                    }
                    break;
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
    private void CheckRemoveCharacter()
    {
        if (CurrentObject != null && nextObject != null &&
            CurrentObject == nextObject && CurrentObject.identity == IdentityType.Ally)
        {
            BattleUIManager.Instance.GenerateCharacterElement(CurrentObject.general, BattleUIManager.Instance.characterSelectUI.transform.position);
            CurrentObject.GetComponent<AllyObject>().ReleaseGeneral();
            UnDrawAreas();
            nextObject = null;
        }
        else return;
    }
    private bool SelectCell()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit != false && hit.collider.TryGetComponent(out BattleMapCell cell))
        {
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
        if (nextCell != null && nextCell.movingObject != null && nextCell.movingObject.general != null)
        {
            nextObject = nextCell.movingObject;
            return true;
        }
        else
        {
            nextObject = null;
            return false;
        }
    }

    private void UpdateCell() 
    {   
        currentCell = nextCell;
        nextCell = null;
    }

    private void UpdateCharacter() 
    {
        CurrentObject = nextObject;
        if(CurrentObject != null && CurrentObject.currentCell != null) originCell = CurrentObject.currentCell;
        else originCell = null;
        nextObject = null;
    }

    private void DrawAreas()
    {
        if (CurrentObject == null || CurrentObject.general == null) return;
        if (false == CurrentObject.canAction) return;
        CurrentObject.DrawAreas();
    }
    private void UnDrawAreas()
    {
        if (CurrentObject == null) return;
        CurrentObject.UnDrawAreas();
    }
    private bool CharacterAction()
    {
        if (CurrentObject == null) return false;
        if (CurrentObject.identity == IdentityType.Enemy) return false;
        if (false == CurrentObject.canAction) return false;

        if (nextObject != null && nextObject.identity == IdentityType.Ally && nextObject != CurrentObject) return false;
        if (false == CurrentObject.attackableArea.Contains(nextCell)) return false;
        if (CurrentObject.TryMove(nextCell))
        {
            return true;
        }
        if (CurrentObject.TryAttack(nextCell))
        {
            BattleUIManager.Instance.ReadyAttack();
            return true;
        }
        return false;
    }
    
    private void CheckMoveCancel()
    {
        if (nextCell == null) { UnDrawAreas(); MoveCancel(); AttackCancel(); return; }
        if (false == CharacterAction()) { UnDrawAreas(); MoveCancel(); AttackCancel(); return; }
    }

    public void MoveCancel()
    {
        CurrentObject.SetPos(originCell);
        CurrentObject.RestoreCosts();
        moveState = MoveState.Ready;
        CurrentObject = null;
        originCell = null;
    }

    public void MoveApply()
    {
        CurrentObject.Move();
        UnDrawAreas();
        CurrentObject = null;
        moveState = MoveState.Ready;
    }

    public void AttackApply()
    {
        CurrentObject.Attack();
        UnDrawAreas();
        CurrentObject = null;
        moveState = MoveState.Ready;
        BattleUIManager.Instance.CancelAttack();
    }

    public void AttackCancel()
    {
        BattleUIManager.Instance.CancelAttack();
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
    public void TurnEnd()
    {
        moveState = MoveState.End;
        TurnManager.Instance.PlayerTurnEnd();
    }

    public void StartFight(MovingObject attacker, MovingObject target)
    {
        attacker.state = MovingObject.State.Attack;
        int distance = BattleMapAStartAlgorithm.Distance(attacker.currentCell, target.currentCell);
        StartCoroutine(FightCoroutine(attacker, target));
        
        if (attacker.range >= distance)
        {
            target.TakeHit(attacker);
        }
        if (target.range >= distance)
        {
            attacker.TakeHit(target);
        }
    }
    IEnumerator FightCoroutine(MovingObject attacker, MovingObject target)
    {
        BattleUIManager.Instance.StartFight(attacker, target);
        yield return new WaitUntil(() => attacker.state == MovingObject.State.Wait); 
        BattleUIManager.Instance.EndFight();
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
                allyObjects.Add(movingObject);
                movingObject.UpdateCurrentCell();
            }
            else if (movingObject.identity == IdentityType.Enemy)
            {
                enemyObjects.Add(movingObject);
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
