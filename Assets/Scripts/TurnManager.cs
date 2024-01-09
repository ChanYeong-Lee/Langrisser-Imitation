using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private State state = State.None;
    public State TurnState { get { return state; } }    
    public static TurnManager Instance { get; private set; }
    public List<MovingObject> aliveObjects = new List<MovingObject>();
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
                state = State.OpenTurn;
                break;
        }

    }

    public void StartBattle()
    {
        state = State.OpenTurn;
        print("TurnStart");
        currentTurn = 0;
        WakeUpObjects();
    }

    private void StartTurn()
    {
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
        }
    }

    private void CalculateAreas()
    {
        foreach (MovingObject movingObject in aliveObjects)
        {
            movingObject.CheckMovableArea();
        }
    }
    private void OpenTurnEnd()
    {
        print("PlayerTurn");
        state = State.PlayerTurn;
    }

    private void PlayerTurn()
    {
    }
    public void PlayerTurnEnd()
    {
        BattleManager.Instance.PlayerTurnEnd();
        state = State.EnemyTurn;
        print("EnemyTurn");
    }

    private void EnemyTurn()
    {
    }
    public void EnemyTurnEnd()
    {
        state = State.CloseTurn;
        print("TurnEnd");
    }

    private void EndTurn()
    {
    }
    private void EndTurnEnd()
    {
        state = State.OpenTurn;
    }
    public void EndBattle()
    {
        state = State.None;
        SceneLoader.Instance.LoadScene("MapScene");
    }
}
