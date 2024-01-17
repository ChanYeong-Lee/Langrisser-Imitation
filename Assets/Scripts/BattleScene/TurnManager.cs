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
        deadObject.currentCell.movingObject = null;
        deadObject.gameObject.SetActive(false);
        CheckStates();
    }

    private void CheckStates()
    {
        if (BattleManager.Instance.CheckAllyDie())
        {
            StartCoroutine(AllyWin());
        }

        if (BattleManager.Instance.CheckEnemyDie())
        {
            StartCoroutine(EnemyWin());
        }
    }

    IEnumerator AllyWin()
    {
        EndTurn();
        EndBattle();
        yield return null;
    }

    IEnumerator EnemyWin()
    {
        EndTurn();
        EndBattle();
        yield return null;
    }
    private void ResetCosts()
    {
        foreach (MovingObject movingObject in aliveObjects)
        {
            movingObject.RestoreCosts();
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
        BattleUIManager.Instance.PlayerTurn();
    }

    private void PlayerTurn()
    {
    }
    public void PlayerTurnEnd()
    {
        BattleUIManager.Instance.PlayerTurnEnd();
        StartCoroutine(EnemyTurnStart());
    }
    IEnumerator EnemyTurnStart()
    {
        yield return new WaitForSeconds(1f);
        TurnState = State.EnemyTurn;
        EnemyBrain.Instance.StartTurn();
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
