using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyObject : MovingObject
{
    private void Awake()
    {
        identity = IdentityType.Ally;
    }

    public void SetGeneral(General general)
    {
        this.general = general;
        this.soldier = Instantiate(general.Soldier);
        Init();
    }
}
