using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TurnTransitionUI : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI turnName;
    public GameObject PlayerTurn;
    public GameObject EnemyTurn;
    private Color originColor;
    public void Init()
    {
        originColor = background.color; 
    }

    public void SetTransitionUI(TurnManager.State turn)
    {
        switch (turn)
        {
            case TurnManager.State.PlayerTurn:
                turnName.text = "플레이어 턴";
                PlayerTurn.SetActive(true);
                break;
            case TurnManager.State.EnemyTurn:
                turnName.text = "적군 턴";
                EnemyTurn.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(UIAnimateCoroutine());
    }

    private void OnDisable()
    {
        PlayerTurn.SetActive(false);
        EnemyTurn.SetActive(false);
        background.color = originColor;
    }

    IEnumerator UIAnimateCoroutine()
    {
        float count = 0;
        while(true)
        {
            count += Time.deltaTime;
            Color color = background.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime);
            background.color = color;
            yield return null;
            if (count >= 1) break;
        }
        gameObject.SetActive(false);
    }
}
