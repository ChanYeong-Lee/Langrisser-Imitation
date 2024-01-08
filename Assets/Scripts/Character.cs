using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum ClassType
{
    SwordMan,
    PikeMan,
    Knight,
    Clergy,
}

public class Character : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnDie; 
    [HideInInspector] public UnityEvent OnHPChange; 
    [HideInInspector] public UnityEvent OnExpChange; 

    public ClassType classType;
    protected float currentHP;
    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            if (currentHP >= maxHP)
            {
                currentHP = maxHP;
            }
            if (currentHP <= 0)
            {
                currentHP = 0;
                Die();
            }
        }
    }
    
    [HideInInspector] public bool alive;

    public float maxHP;
    public float damage;
    public float defense;

    public int range;
    public int moveCost;

    protected void OnEnable()
    {
        alive = true;
        CurrentHP = maxHP;
    }

    private void Die()
    {
        alive = true;
    }
}
