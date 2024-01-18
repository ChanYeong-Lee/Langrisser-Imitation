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
    public GameObject playerTurn;
    public GameObject enemyTurn;
    private GameObject currentTurn;
    private Color originColor;
    public void Init()
    {
        originColor = background.color;
        playerTurn.SetActive(false);
        enemyTurn.SetActive(false);
        currentTurn = null;
    }

    public void SetTransitionUI(TurnManager.State turn)
    {
        switch (turn)
        {
            case TurnManager.State.PlayerTurn:
                turnName.text = "플레이어 턴";
                currentTurn = playerTurn;
                break;
            case TurnManager.State.EnemyTurn:
                turnName.text = "적군 턴";
                currentTurn = enemyTurn;
                break;
            default:
                break;
        }
        currentTurn.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(UIAnimateCoroutine());
    }

    private void OnDisable()
    {
        playerTurn.SetActive(false);
        enemyTurn.SetActive(false);
        currentTurn.GetComponent<RectTransform>().localScale = Vector3.one;
        currentTurn = null;
        background.color = originColor;
    }

    IEnumerator UIAnimateCoroutine()
    {
        float count = 0;
        while(true)
        {
            count += Time.deltaTime;
            if (count >= 1) break;
            Color color = new Color(0, 0, 0, 0);
            color.a = Time.deltaTime / 2;
            background.color += color;
            Vector3 scale = new Vector3(Time.deltaTime / 2, Time.deltaTime / 2, 0);
            currentTurn.GetComponent<RectTransform>().localScale += scale;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
