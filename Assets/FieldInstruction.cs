using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInstruction : MonoBehaviour
{
    [HideInInspector] public GeneralInstruction fieldInstruction;
    public void Init()
    {
        fieldInstruction = GetComponent<GeneralInstruction>();
        BattleManager.Instance.onObjectChange.AddListener(UpdateInstruction);
        UpdateInstruction();
    }

    [Header("Layouts")]
    public RectTransform vertical;
    public RectTransform horizontal;
    public RectTransform topHorizontal;


    private void ChangeState()
    {
        RectTransform parent = new RectTransform();
        if (BattleManager.Instance.state == BattleManager.State.Ready) parent = vertical;
        else if (BattleManager.Instance.state == BattleManager.State.Battle)
        {
            parent = horizontal;
            if (BattleManager.Instance.CurrentObject != null &&
                BattleManager.Instance.CurrentObject.identity == IdentityType.Ally &&
                BattleManager.Instance.CurrentObject.canAction) parent = topHorizontal;
        }
        fieldInstruction.SetParent(parent);
    }

    public void UpdateInstruction()
    {
        ChangeState();
        if (BattleManager.Instance.CurrentObject == null ||
             BattleManager.Instance.CurrentObject.general == null) { gameObject.SetActive(false); return; }
        gameObject.SetActive(true);
        fieldInstruction.SetInstruction(BattleManager.Instance.CurrentObject);
    }

    public void ReadyAttack()
    {
        gameObject.SetActive(false);
    }
    public void CancelAttack()
    {
        gameObject.SetActive(true);
    }
}
