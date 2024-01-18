using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleReadyUI : MonoBehaviour
{
    public Button battleStartButton;
    public CharacterSelectUI characterSelectUI;
    public void Init()
    {
        battleStartButton.onClick.AddListener(BattleStart);
        characterSelectUI.Init();
    }

    public void BattleStart()
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
            BattleManager.Instance.StartBattle();
            gameObject.SetActive(false);
        }
    }
}
