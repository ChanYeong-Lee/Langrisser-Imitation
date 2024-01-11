using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    public BattleMapCell currentCell;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        state = State.Ready;
        TurnManager.Instance.onPlayerTurnEnd.AddListener(PlayerTurnEnd);
    }

    public void TestBattle()
    {
        for (int i = 0; i < allyObjects.Count; i++)
        {
            allyObjects[i].SetGeneral(PlayerData.Instance.generals[i]);
        }
    }
    private void Update()
    {
        switch (state)
        {
            case State.Ready:
                break;
            case State.Battle:
                if (TurnManager.Instance.TurnState == TurnManager.State.PlayerTurn)
                    CharacterMove();
                break;
        }
        SelectCharacter();
    }
    
    public void StartBattle()
    {
        state = State.Battle;
        TurnManager.Instance.StartBattle();
    }

    private void SelectCharacter()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit == false) return;
            if (hit.collider.TryGetComponent(out MovingObject nextObject))
            {
                if (currentObject != null)
                {
                    currentObject.UnDrawAreas();
                }
                currentObject = nextObject;
                currentObject.DrawAreas();
            }
            if (hit.collider.TryGetComponent(out BattleMapCell cell))
            {
                if (currentObject != null)
                {
                    currentObject.UnDrawAreas();
                }
                currentCell = cell;
                currentObject = null;
            }
        }
    }

    private void CharacterMove()
    {
        if (currentObject != null && currentObject.identity == IdentityType.Ally)
        {
            if (Input.GetMouseButtonUp(0))
            {
                print("GGOODD");
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit == false) return;
                if (hit.collider.TryGetComponent(out BattleMapCell cell))
                {
                    currentObject.TryMove(cell);
                }
            }
        }
    }
    public void PlayerTurnEnd()
    {
        if (currentObject != null)
        {
            currentObject.UnDrawAreas();
        }
    }

    public void SetBattleMap(BattleMap battleMap)
    {
        currentBattleMap = battleMap;
        movingObjects = currentBattleMap.movingObjects;
        foreach (MovingObject movingObject in movingObjects)
        {
            if (movingObject.identity == IdentityType.Ally)
            {
                allyObjects.Add(movingObject as AllyObject);
            }
            else if (movingObject.identity == IdentityType.Enemy)
            {
                enemyObjects.Add(movingObject as EnemyObject);
            }
            movingObject.currentMap = this.currentBattleMap;
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
