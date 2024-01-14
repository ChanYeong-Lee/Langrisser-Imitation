using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum ClassType
{
    SwordMan    = 0,
    PikeMan     = 1,
    SwordHorse  = 2,
    Clergy      = 3,
}

public class Character : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnDie; 
    [HideInInspector] public UnityEvent OnHPChange; 
    
    [SerializeField] private ClassType classType;
    public ClassType ClassType { get { return classType; } }

    public float currentHP;
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
    
    private bool alive;
    [HideInInspector] public bool Alive { get { return alive; } set { alive = value; if (alive) CurrentHP = maxHP; OnHPChange?.Invoke(); } }

    public float maxHP;
    public float damage;
    public float defense;

    public int range;
    public int moveCost;

    private void Die()
    {
        alive = false;
        OnDie?.Invoke();
    }

    public float TakeHit(float damage)
    {
        print($"{gameObject.name} is Take Hit");
        float remainDamage = 0;
        if (damage > currentHP)
        {
            remainDamage = damage - currentHP;
        }
        CurrentHP -= damage;
        return remainDamage;
    }
}
