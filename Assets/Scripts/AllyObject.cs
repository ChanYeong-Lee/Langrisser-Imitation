using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyObject : MovingObject
{
    private void OnEnable()
    {
        identity = IdentityType.Ally;
    }

    public void SetGeneral(General general)
    {
        this.general = general;
        general.transform.parent = this.transform;
        this.soldier = general.Soldier;
        soldier.transform.parent = this.transform;
        Init();
    }
}
