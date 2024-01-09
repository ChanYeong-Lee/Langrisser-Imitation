using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoldierType
{
    SwordMan,
    PikeMan,
    SwordHorse,
    Clergy,
}
public class Soldier : Character
{
    public SoldierType soldierType;

}
