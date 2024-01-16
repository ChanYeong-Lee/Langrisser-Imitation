using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance { get; private set; }
    public GameObject battleReadyUI;
    public MoveReadyUI moveReadyUI;
    public CharacterSelectUI characterSelectUI;
    public FieldInstruction fieldInstruction;
    public AttackReadyUI attackReadyUI;
    private void Awake()
    {
        Instance = this;
    }

    public void ReadyBattle()
    {
        battleReadyUI.SetActive(true);
        fieldInstruction.Init();
        moveReadyUI.Init();
        attackReadyUI.Init();
    }

    public void StartBattle()
    {
        bool isReady = false;
        foreach (MovingObject movingObject in BattleManager.Instance.allyObjects)
        {
            if (movingObject.general != null)
            {
                isReady = true;
                break;
            }
        }
        if (isReady)
        {
            battleReadyUI.SetActive(false);
            BattleManager.Instance.StartBattle();
        }
    }
    public void GenerateCharacterElement(General general, Vector2 pos)
    {
        characterSelectUI.GenerateElement(general, pos);
    }
    

    public void ReadyAttack()
    {
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
}
