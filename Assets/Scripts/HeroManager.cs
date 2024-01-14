using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroManager : MonoBehaviour
{
    public static HeroManager Instance { get; private set; }
    public General currentGeneral;
    public UnityEvent onChangeGeneral;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentGeneral = PlayerData.Instance.generals[0];
    }

    public void ChangeGeneral(General general)
    {
        currentGeneral = general;
        onChangeGeneral?.Invoke();
    }
}