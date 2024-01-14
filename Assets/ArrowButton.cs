using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArrowButton : MonoBehaviour
{
    public enum Direction 
    { 
        Left    = -1,
        Right   =  1
    };

    public Direction direction;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickButton);
    }

    public void ClickButton()
    {
        int currentIndex = PlayerData.Instance.generals.IndexOf(HeroManager.Instance.currentGeneral);
        int nextIndex = (currentIndex + (int)direction + PlayerData.Instance.generals.Count) % PlayerData.Instance.generals.Count;
        HeroManager.Instance.ChangeGeneral(PlayerData.Instance.generals[nextIndex]);
    }

}
