using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public enum State
    {
        None,
        OpenTurn,
        PlayerTurn,
        EnemyTurn,
        CloseTurn,
    }
    public static TurnManager Instance { get; private set; }
    [SerializeField] private State state;
    public State TurnState { get { return state; } set { state = value; onTurnChange?.Invoke(); } }
    public List<MovingObject> aliveObjects = new List<MovingObject>();
    public UnityEvent onTurnChange;
    private int currentTurn;
    public int CurrentTurn { get { return currentTurn; } }
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        switch (state)
        {
            case State.None:
                break;
            case State.OpenTurn:
                StartTurn();
                ResetCosts();
                CalculateAreas();
                OpenTurnEnd();
                break;
            case State.PlayerTurn:
                PlayerTurn();
                break;
            case State.EnemyTurn:
                EnemyTurn();
                break;
            case State.CloseTurn:
                EndTurn();
                EndTurnEnd();
                break;
        }

    }

    public void StartBattle()
    {
        TurnState = State.OpenTurn;
        print("StartBattle");
        currentTurn = 0;
        WakeUpObjects();
    }

    private void StartTurn()
    {
        print("StartTurn");
        currentTurn++;
    }
    private void WakeUpObjects()
    {
        foreach (MovingObject movingObject in BattleManager.Instance.movingObjects)
        {
            movingObject.Alive = true;
            aliveObjects.Add(movingObject);
            movingObject.OnDie.AddListener(ObjectDie);
        }
    }

    private void ObjectDie(MovingObject deadObject)
    {
        aliveObjects.Remove(deadObject);
        Destroy(deadObject.gameObject);
        CheckStates();
    }

    private void CheckStates()
    {
        if (BattleManager.Instance.CheckAllyDie())
        {
            AllyWin();
        }

        if (BattleManager.Instance.CheckEnemyDie())
        {
            EnemyWin();
        }
    }

    private void AllyWin()
    {
        EndTurn();
        EndBattle();
    }

    private void EnemyWin()
    {
        EndTurn();
        EndBattle();
    }
    private void ResetCosts()
    {
        foreach (MovingObject movingObject in aliveObjects)
        {
            movingObject.currentMoveCost = movingObject.moveCost;
            movingObject.canAction = true;
        }
    }

    private void CalculateAreas()
    {
        foreach (MovingObject movingObject in aliveObjects)
        {
            movingObject.CheckMovableArea();
            movingObject.CheckAttackableArea();
        }
    }
    private void OpenTurnEnd()
    {
        print("PlayerTurn");
        TurnState = State.PlayerTurn;
    }

    private void PlayerTurn()
    {
    }
    public void PlayerTurnEnd()
    {
        TurnState = State.EnemyTurn;
        print("EnemyTurn");
    }

    private void EnemyTurn()
    {
    }
    public void EnemyTurnEnd()
    {
        state = State.CloseTurn;
        print("EnemyTurnEnd");
    }

    private void EndTurn()
    {
    }
    private void EndTurnEnd()
    {
        TurnState = State.OpenTurn;
    }
    public void EndBattle()
    {
        TurnState = State.None;
        SceneLoader.Instance.LoadScene("MapScene");
    }
}
