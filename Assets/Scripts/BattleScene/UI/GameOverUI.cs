using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Image background;
    public GameObject allyWinWord;
    public GameObject enemyWinWord;
    public Button continueButton;
    public TextMeshProUGUI buttonText;
    GameObject currentWord;
    Color backgroundColor;
    Color wordColor;
    
    public void Init()
    {
        backgroundColor = new Color(0, 0, 0, 0);
        wordColor = new Color(255, 255, 255, 0);
        ResetColor();
        continueButton.onClick.AddListener(ButtonClick);
    }

    private void OnEnable()
    {
        StartCoroutine(UIAnimateCoroutine());
    }

    private void OnDisable()
    {
        ResetColor();
        allyWinWord.SetActive(false);
        enemyWinWord.SetActive(false);
        continueButton.gameObject.SetActive(false);
        currentWord = null;
    }

    public void SetWord(IdentityType identity)
    {
        switch (identity)
        {
            case IdentityType.Ally:
                allyWinWord.SetActive(true);
                currentWord = allyWinWord;
                break;
            case IdentityType.Enemy:
                enemyWinWord.SetActive(true);
                currentWord = enemyWinWord;
                break;
        }
    }

    IEnumerator UIAnimateCoroutine()
    {
        MaskableGraphic[] masks = currentWord.GetComponentsInChildren<MaskableGraphic>();
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= 2f) break;
            Color transparency = new Color(0, 0, 0, Time.deltaTime);
            if (0 <= time && time < 1f) background.color += transparency;
            else
            {
                if (false == continueButton.gameObject.activeSelf) continueButton.gameObject.SetActive(true);
                for (int i = 0; i < masks.Length; i++)
                {
                    masks[i].color += transparency;
                }
                buttonText.color += transparency;
            }
            yield return null;
        }
    }

    private void ResetColor()
    {
        MaskableGraphic[] masks = GetComponentsInChildren<MaskableGraphic>(true);
        for (int i = 0; i < masks.Length; i++)
        {
            masks[i].color = wordColor;
        }
        background.color = backgroundColor;
    }

    public void ButtonClick()
    {
        TurnManager.Instance.ExitBattle();
    }
}
