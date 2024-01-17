using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance { get; private set; }
    public BattleReadyUI battleReadyUI;
    public MoveReadyUI moveReadyUI;
    public CharacterSelectUI characterSelectUI;
    public FieldInstruction fieldInstruction;
    public AttackReadyUI attackReadyUI;
    public GameObject turnEndButton; 
    public TurnTransitionUI turnTransitionUI;
    public FightUI fightUI;

    private void Awake()
    {
        Instance = this;
    }

    public void ReadyBattle()
    {
        battleReadyUI.Init();
        fieldInstruction.Init();
        moveReadyUI.Init();
        attackReadyUI.Init();
        turnTransitionUI.Init();
        battleReadyUI.gameObject.SetActive(true);
    }

    public void GenerateCharacterElement(General general, Vector2 pos)
    {
        characterSelectUI.GenerateElement(general, pos);
    }

    
    public void PlayerTurn()
    {
        turnEndButton.SetActive(true);
        turnTransitionUI.SetTransitionUI(TurnManager.State.PlayerTurn);
        turnTransitionUI.gameObject.SetActive(true);
    }


    public void PlayerTurnEnd()
    {
        turnEndButton.SetActive(false);
        turnTransitionUI.SetTransitionUI(TurnManager.State.EnemyTurn);
        turnTransitionUI.gameObject.SetActive(true);
    }

    public void ReadyAttack()
    {
        StopAllCoroutines();
        StartCoroutine(ReadyAttackCoroutine());
    }
    IEnumerator ReadyAttackCoroutine()
    {
        yield return new WaitUntil(() => BattleManager.Instance.CurrentObject.state == MovingObject.State.None);
        fieldInstruction.ReadyAttack();
        moveReadyUI.ReadyAttack();
        attackReadyUI.gameObject.SetActive(true);
        attackReadyUI.SetInstructions(BattleManager.Instance.CurrentObject, BattleManager.Instance.NextObject);
    }
    public void CancelAttack()
    {
        fieldInstruction.CancelAttack();
        moveReadyUI.CancelAttack();
        attackReadyUI.gameObject.SetActive(false);
    }

    public void StartFight(MovingObject attacker, MovingObject target)
    {
        fightUI.gameObject.SetActive(true);
        fightUI.StartFight(attacker, target);
    }

    public void EndFight()
    {
        fightUI.gameObject.SetActive(false);
    }
}
