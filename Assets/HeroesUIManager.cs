using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesUIManager : MonoBehaviour
{
    public GameObject heroInstruction;
    public GameObject heroSelectUI;
    public GameObject instructionButton;
    public GameObject instructionPanel;
    public GameObject instructionOptions;

    Vector2 instructionFirstPos;
    Vector2 instructionSecondPos;

    Vector2 firstCamPos;
    Vector2 secondCamPos;
    public enum State { HeroesGrid, Instruction };
    public State state;
    private void Awake()
    {
        firstCamPos = new Vector2(800, 450);
        secondCamPos = new Vector2(1425, 450);
        instructionFirstPos = Vector2.zero;
        instructionSecondPos = new Vector2(-225, 0);
        state = State.HeroesGrid;
        Camera.main.transform.position = new Vector3(firstCamPos.x, firstCamPos.y, Camera.main.transform.position.z);
        heroInstruction.GetComponent<RectTransform>().anchoredPosition = instructionFirstPos;
        heroSelectUI.SetActive(true);
        instructionButton.SetActive(true);
        instructionPanel.SetActive(false);
        instructionOptions.SetActive(false);
    }

    public void ClickInstructionButton()
    {
        StopAllCoroutines();
        heroSelectUI.SetActive(false);
        instructionButton.SetActive(false);
        instructionPanel.SetActive(true);
        instructionOptions.SetActive(true);
        StartCoroutine(CameraMoveCoroutine(secondCamPos));
        StartCoroutine(UIMoveCoroutine(heroInstruction, instructionSecondPos));
        state = State.Instruction;
    }
    public void ClickReturnButton()
    {
        StopAllCoroutines();
        switch (state)
        {
            case State.HeroesGrid:
                SceneLoader.Instance.LoadScene("MapScene");
                break;
            case State.Instruction:
                heroSelectUI.SetActive(true);
                instructionButton.SetActive(true);
                instructionPanel.SetActive(false);
                instructionOptions.SetActive(false);
                StartCoroutine(CameraMoveCoroutine(firstCamPos));
                StartCoroutine(UIMoveCoroutine(heroInstruction, instructionFirstPos));
                state = State.HeroesGrid;
                break;
        }
    }

    IEnumerator CameraMoveCoroutine(Vector2 pos)
    {
        Vector3 pos3 = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
        while (true)
        {
            if (Vector2.Distance(Camera.main.transform.position, pos3) < 0.5f) break;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pos3, Time.deltaTime * 2);
            yield return null;
        }
        Camera.main.transform.position = pos3;
    }
    IEnumerator UIMoveCoroutine(GameObject UI, Vector2 pos)
    {
        RectTransform rectTransform = UI.GetComponent<RectTransform>();
        while (true)
        {
            if (Vector2.Distance(rectTransform.anchoredPosition, pos) < 0.5f) break;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, pos, Time.deltaTime * 2);
            yield return null;
        }
        rectTransform.anchoredPosition = pos;
    }

}
