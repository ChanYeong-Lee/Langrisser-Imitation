using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveReadyUI : MonoBehaviour
{
    public void Init()
    {
        BattleManager.Instance.onObjectChange.AddListener(UpdateUI);
    }
    public void UpdateUI()
    {
        if (BattleManager.Instance.state == BattleManager.State.Battle &&
            BattleManager.Instance.CurrentObject != null &&
            BattleManager.Instance.CurrentObject.identity == IdentityType.Ally &&
            BattleManager.Instance.CurrentObject.canAction)          
        {
            gameObject.SetActive(true);
        }
        else gameObject.SetActive(false);
    }

    public void ReadyAttack()
    {
        gameObject.SetActive(false);
    }

    public void CancelAttack()
    {
        gameObject.SetActive(false);
    }
}
