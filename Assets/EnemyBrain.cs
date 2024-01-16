using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public List<MovingObject> allyObjects;
    public List<MovingObject> enemyObjects;

    public void Init()
    {
        allyObjects = BattleManager.Instance.enemyObjects;
        enemyObjects = BattleManager.Instance.allyObjects;
    }

}
