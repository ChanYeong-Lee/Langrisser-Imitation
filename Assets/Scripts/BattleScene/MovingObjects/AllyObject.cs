using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyObject : MovingObject
{
    protected override void Awake()
    {
        base.Awake();
        identity = IdentityType.Ally;
    }

    public void SetGeneral(General general)
    {
        if (this.general != null)
        {
            Release();
            PlayerData.Instance.ReturnGeneral(this.general);
            Destroy(soldier.gameObject);
        }
        this.general = general;
        this.soldier = Instantiate(general.Soldier);
        general.gameObject.SetActive(true);
        general.Alive = true;
        soldier.Alive = true;
        InitMovingObject();
    }

    public void ReleaseGeneral()
    {
        if (this.general == null) return;
        general.Alive = false;
        soldier.Alive = false;
        Release();
        PlayerData.Instance.ReturnGeneral(this.general);
        Destroy(soldier.gameObject);
        this.general = null;
        this.soldier = null;
    }
}
